using Player;
using System.Collections.Generic;
using TilemapScripts;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    #region Fields

    [Header("Player")]
    public Controller player;
    public MarkerManager markerManager;

    [Header("Scene Specific")]
    public EnviroManager enviroManager;
    public Tilemap tm;

    [Header("Global")]
    public GameObject itemVisual; // for always having item infront of all UI
    public TilemapInfoManager tilemapInfoManager;

    // Hidden : set within script

    [HideInInspector] public static GameManager Instance;
    [HideInInspector] public Reader reader;
    [HideInInspector] public Inventory.Manager inventory;
    [HideInInspector] public TilemapScripts.CropsManager cropsManager;
    [HideInInspector] public TimeController timeController;
    [HideInInspector] public OnScreenMessageSystem screenMessageSystem;
    [HideInInspector] public SceneManager sceneManager;
    [HideInInspector] public PlaceableObjectsReferenceManager placeableObjectsManager;

    #endregion

    #region Runtime
    /// <summary>
    /// Class is a singleton, only one should exist at ALL times
    /// </summary>
    void Awake()
    {
        if (Instance == null) // if there is no instance already
        {
            Instance = this;
        }
        else if (Instance != this) // if there is already an instance and it's not `this` instance
        {
            Destroy(gameObject); // destroy the gameobject, this component is attached to
        }

        inventory = GetComponent<Inventory.Manager>();
        reader = GetComponent<Reader>();
        cropsManager = GetComponent<TilemapScripts.CropsManager>();
        timeController = GetComponent<TimeController>();
        screenMessageSystem = GetComponent<OnScreenMessageSystem>();
        placeableObjectsManager = GetComponent<PlaceableObjectsReferenceManager>();
    
    }

    private void Start()
    {
        SetPosition();
        FindTilemaps();
    }

    #endregion

    public void SetPosition() // fix spawn
    {
        tm = GameObject.Find("0").GetComponent<Tilemap>();
        Collider2D col = tm.gameObject.GetComponent<Collider2D>();

        Vector3 pos = new Vector3Int(75, 75, 0);

        while (!col.bounds.Contains(pos))
        {
            pos = col.ClosestPoint(pos) - new Vector2(0.5f, 0.5f);
        }

        player.gameObject.transform.position = pos;
    }

    #region Helpers

    public void FindTilemaps()
    {
        GameObject grid = GameObject.Find("Grid Base");

        if (grid == null) { return; }

        tilemapInfoManager = grid.GetComponent<TilemapInfoManager>();
    }

    #endregion
}
