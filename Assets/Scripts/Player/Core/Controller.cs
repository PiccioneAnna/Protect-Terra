using Interacts;
using System.Collections.Generic;
using TilemapScripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// Class handles movement, actions, and main references to other systems
namespace Player
{
    public class Controller : MonoBehaviour
    {
        #region Fields
        
        [HideInInspector] public Rigidbody2D _rb;
        [HideInInspector] public CapsuleCollider2D _collider;
        [HideInInspector] public AnimManager animator;
        [HideInInspector] public PlayerControls playerControls;
        
        [Header("Gameobject References")]
        public SpriteRenderer equipedSprite;
        public WeaponPoint weaponPoint;
        [SerializeField] ToolActions.Base onTilePickUp;
        [SerializeField] ItemHighlight itemHighlight;

        // Manager References
        [HideInInspector] public GameManager gameManager;
        [HideInInspector] public Inventory.Manager inventoryManager;
        [HideInInspector] public MarkerManager markerManager;
        [HideInInspector] TilemapScripts.Reader tilemapReader;
        [HideInInspector] public static Controller controller;
        [HideInInspector] public Character character;
        [HideInInspector] public UIManager uiManager;

        private InputAction move;
        private InputAction interact;

        [Header("Conditions")]
        public bool canFire;
        public bool canDoAction;
        private float attackTimer = 0f;
        private float timeBetweenShots = .5f;
        private bool useGrid;
        private bool selectable;
        private bool multiGrid;
        private bool place;
        public bool isInteract = false;
        public bool isUIOpen;

        [Header("Stats")]
        public float _speed = 3f;
        private float _currSpeed;
        public float _sprintMultiplier = 1.6f;
        public float _timeBetweenActions = .5f;
        public Vector2 sizeOfIA; // static for all tools for now
        float maxDistance;
        private Vector2 _movementInput;
        private Vector2 _mousePos;
        private Vector2 _moveDirection = Vector2.zero;
        private float offsetDistance = 1.2f;
        public float passiveRegenE = .50f;
        public float passiveRegenH = .10f;
        public float actionTimer;
       
        Vector3Int selectedTilePosition;
        List <Vector3Int> selectedTiles;

        [Header("Data")]
        public Data.Item selectedItem;
        public ItemForPickup[] itemsToPickup;

        #endregion

        #region Runtime
        private void Awake()
        {
            controller = this;
            character = GetComponent<Character>();
            playerControls = new();
            maxDistance = sizeOfIA.x * sizeOfIA.y;
            selectedTiles = new List<Vector3Int>();
            weaponPoint = GetComponentInChildren<WeaponPoint>();

            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CapsuleCollider2D>();
            animator = GetComponentInChildren<AnimManager>();
            uiManager = GetComponent<UIManager>();
        }

        // Start is called before the first frame update
        void Start()
        {
            gameManager = GameManager.Instance;
            actionTimer = 0f;

            //// Reference each manager in the GameManager
            inventoryManager = gameManager.inventory;
            tilemapReader = gameManager.reader;
            markerManager = gameManager.markerManager;

            PickupItemList();

            HandleSelection();
        }

        // Update is called once per frame
        void Update()
        {
            // Movement related
            _moveDirection = move.ReadValue<Vector2>();
            Animate();

            HandleSelection();
            RangedAttackMath();

            uiManager.HandleUIInteractions();

            actionTimer += Time.deltaTime;
            if (actionTimer > _timeBetweenActions)
            {
                canDoAction = true;
                actionTimer = 0f;
            }
        }

        void FixedUpdate()
        {
            ApplyMovement();

            // Passive Regen
            character.Rest(passiveRegenE);
            character.Heal(passiveRegenH);

            if (Input.GetMouseButtonDown(0))
            {
                WeaponAction();
            }

            if (Input.GetMouseButton(0))
            {
                if (!isInteract && !isUIOpen && character.stamina.currVal > 0 && canDoAction)
                {
                    if (!isInteract && !useGrid)
                    {
                        UseToolWorld();
                        character.GetTired(1);
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (selectedItem == null) { Interact(); }

                if (!isInteract && useGrid)
                {
                    UseToolGrid();
                }
            }
        }

        private void OnEnable()
        {
            move = playerControls.Player.Move;
            move?.Enable();
        }
        private void OnDisable()
        {
            move?.Disable();
        }
        #endregion

        #region Animator Calls
        public void Animate()
        {
            if (animator != null)
            {
                animator.dirValue = _moveDirection;
                animator.UpdateAnimation();
            }
        }
        #endregion

        #region Movement
        protected void ApplyMovement()
        {
            _currSpeed = Keyboard.current.shiftKey.isPressed ? _speed * _sprintMultiplier : _speed;

            _rb.velocity = new Vector2(_moveDirection.x * _currSpeed, _moveDirection.y * _currSpeed);
        }
        #endregion

        #region Tools
        private bool UseToolWorld()
        {
            if (character.stamina.currVal <= 0) { return false; };

            Vector2 position = _rb.position;

            if (selectedItem == null)
            {
                PickUpTile();
                return true;
            }

            if (selectedItem.onAction == null) { return false; }

            bool complete = selectedItem.onAction.OnApply(position);

            // Checks if item used can be removed from inventory
            if (complete)
            {
                if (selectedItem.onItemUsed != null)
                {
                    selectedItem.onItemUsed.OnItemUsed(selectedItem, inventoryManager);
                }
            }

            return complete;
        }

        public MarkerValidity VisualizeToolTile(MarkerTile marker)
        {
            bool complete = selectedItem.onTileMapAction.VisualizeOnApplyToTileMap(marker.position, tilemapReader, selectedItem);

            Debug.Log(complete);

            return complete ? MarkerValidity.valid : MarkerValidity.invalid;
        }

        public void UseToolGrid()
        {
            if (selectable)
            {
                if (selectedItem.onTileMapAction == null) { return; }
                if (!markerManager.isShow) { return; }

                foreach (MarkerTile marker in markerManager.markers)
                {
                    bool complete = selectedItem.onTileMapAction.OnApplyToTileMap(marker.position, tilemapReader, selectedItem);

                    // Checks if item used can be removed from inventory
                    if (complete)
                    {
                        if (selectedItem.onItemUsed != null)
                        {
                            selectedItem.onItemUsed.OnItemUsed(selectedItem, inventoryManager);
                        }
                    }
                }

                markerManager.ClearTilemap();
            }
        }

        private void Interact()
        {
            Debug.Log("tryna interact");

            _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] colliders = Physics2D.OverlapPointAll(_mousePos);

            foreach (Collider2D c in colliders)
            {
                if (c.TryGetComponent<Interactable>(out var obj))
                {
                    Debug.Log("Found an interactable...");
                    obj.Interact(controller);
                }
            }
        }

        private void WeaponAction()
        {
            //Player can only attack withitems marked as weapons
            if (selectedItem != null && selectedItem.isWeapon)
            {
                Attack(selectedItem.damage);
            }
        }
        #endregion

        #region Interactions
        public void PickupItem(int id)
        {
            inventoryManager.AddItem(itemsToPickup[id].item);
        }
        public void PickupItemList()
        {
            // Items player starts out with
            for (int i = 0; i < itemsToPickup.Length; i++)
            {
                for(int j = 0; j < itemsToPickup[i].count; j++)
                {
                    PickupItem(i);
                }
            }
        }
        private void SelectTile()
        {
            selectedTilePosition = tilemapReader.GetGridPosition(tilemapReader.tilemap,Input.mousePosition, true);
            Marker();
        }
        private void PickUpTile()
        {
            if (onTilePickUp == null) { return; }
            onTilePickUp.OnApply(new Vector2(_mousePos.x, _mousePos.y - .5f));
        }
        private void Marker()
        {
            markerManager.markedCellPosition = selectedTilePosition;
            //itemHighlight.cellPosition = selectedTilePosition;

            multiGrid = markerManager.isMultiple;
            place = markerManager.isPlace;
        }
        private void HandleSelection()
        {
            selectedItem = inventoryManager.selectedItem;

            if (selectedItem != null && 
                selectedItem.image != null) { equipedSprite.sprite = selectedItem.image; }
            else
            {
                equipedSprite.sprite = null;
            }

            if (selectedItem == null || !selectedItem.usesGrid)
            {
                useGrid = false;
                markerManager.Show(false);
                return;
            }

            // Checks if grid needs to be displayed
            useGrid = true;
            CanSelectCheck();
            SelectTile();
        }
        public void Attack(float damage)
        {
            if(selectedItem.isMelee)
            {
                MeleeAttack(damage);
            }
            else if(selectedItem.isRanged) 
            {
                if (canFire)
                {
                    RangedAttack(damage);
                    canFire = false;
                }
            }
        }

        private void MeleeAttack(float damage)
        {
            Vector2 position = _rb.position + _moveDirection * offsetDistance;

            Collider2D[] targets = Physics2D.OverlapBoxAll(position, sizeOfIA, 0f);

            foreach (Collider2D c in targets)
            {
                if (c.TryGetComponent<Damageable>(out var damageable))
                {
                    damageable.TakeDamage(damage);
                    break;
                }

                Damageable damageable1 = c.gameObject.GetComponentInParent<Damageable>();

                if (damageable1 != null)
                {
                    damageable1.TakeDamage(damage);
                    break;
                }
            }
        }

        // static damage for now got lazy
        private void RangedAttack(float damage)
        {
            if(selectedItem.projectile != null)
            {
                weaponPoint.Fire(selectedItem.projectile, damage);
            }
            else
            {
                Debug.Log("Projectile prefab not set for" + selectedItem.name);
            }
        }

        private void RangedAttackMath()
        {
            if (!canFire)
            {
                attackTimer += Time.deltaTime;
                if(attackTimer > timeBetweenShots)
                {
                    canFire = true;
                    attackTimer = 0;
                }
            }
        }

        #endregion

        #region Checks
        // Method checks if it is possible for the user to select the tile 
        // based on its position and the camera's posiiton
        void CanSelectCheck()
        {
            Vector2 characterPosition = transform.position;
            Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selectable = Vector2.Distance(characterPosition, cameraPosition) < maxDistance;
            markerManager.Show(selectable);
            //itemHighlight.CanSelect = selectable;
        }
        #endregion
    }
}

