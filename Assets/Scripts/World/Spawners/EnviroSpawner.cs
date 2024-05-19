using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static EnviroManager;

public class EnviroSpawner : MonoBehaviour
{
    [Header("Enviro Stats")]
    public int minSpawnCount;
    public int maxSpawnCount;
    public int spawnCount;

    [HideInInspector] public EnviroSpawner enviroSpawner;
    [HideInInspector] public EnviroManager enviroManager;
    [Header("Components")]
    public Tilemap refTilemap;
    public GameObject spawnParent;

    [Header("Spawnable Objects")]
    public GameObject[] objects;
    public List<GameObject> spawnedObjects;
    public float minScale = .5f;
    public float maxScale = 2.5f;
    public int layer = 2;

    private Vector3 position;
    private Quaternion rotation;
    private Vector3 scale;

    private System.Random random;

    BoundsInt bounds;
    [HideInInspector] public List<Vector2> takenPositions;

    // Start is called before the first frame update
    public void Init()
    {
        enviroSpawner = this;
        enviroManager = GameManager.Instance.enviroManager;

        CreateBounds();

        // Random Spawn Count
        random = new System.Random();
        spawnCount = random.Next(maxSpawnCount) + minSpawnCount;
        spawnedObjects = new List<GameObject>();

        if (objects.Length <= 0 ) { return; }

        for (int i = 0; i < spawnCount; i++)
        {
            SpawnObject(RandomObjectPrefab());
        }

        EnumHelper.SetLayerMaskAllChildren(transform, gameObject.layer);
        EnumHelper.SetSortingOrderAllChildren(spawnedObjects, layer);
    }

    public GameObject SpawnObject(GameObject go, Vector3Int pos = new Vector3Int())
    {
        RandomPosition();
        RandomScale();

        position = pos != default ? pos : position;

        // Doesn't spawn if already object
        if (enviroManager.CheckObjectPosition(position))
        {
            return null;
        }

        GameObject temp = Instantiate(go, new Vector3(position.x + .5f, position.y + .5f, 0) + transform.position, rotation, this.transform);
        temp.transform.localScale = scale;
        temp.transform.parent = spawnParent.transform;

        if(temp.TryGetComponent(out TreeNode tre))
        {
            tre.PreExistingTreeNode();
        }

        enviroManager.spawnedObjects.Add(CreateResourceObject(position, temp));

        return temp;
    }

    public GameObject NewTreeObject(GameObject go, Vector3Int pos = new Vector3Int())
    {
        position = pos != default ? pos : position;

        // Doesn't spawn if already object
        if (enviroManager.CheckObjectPosition(position))
        {
            return null;
        }

        GameObject temp = Instantiate(go, new Vector3(position.x + .5f, position.y + .5f, 0) + transform.position, rotation, this.transform);
        temp.transform.localScale = scale;
        temp.transform.parent = spawnParent.transform;

        temp.GetComponent<TreeNode>().NewTreeNode();

        enviroManager.spawnedObjects.Add(CreateResourceObject(position, temp));

        return temp;
    }

    #region Helper Methods

    private void RandomScale()
    {
        float s = UnityEngine.Random.Range(minScale, maxScale);
        scale = new Vector3(s, s, 0);
    }

    private void RandomPosition()
    {
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        position = new Vector2((int)randomX + .5f, (int)randomY + .5f);
    }

    // Get a random object to spawn;
    private GameObject RandomObjectPrefab()
    {
        return objects[random.Next(objects.Length)];
    }

    private bool RandomSign()
    {
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            return true;
        }
        return false;
    }

    private void CreateBounds()
    {
        bounds = TilemapGenerator.ReturnTilemapInfo(refTilemap);
    }

    private ObjectInfo CreateResourceObject(Vector2 pos, GameObject o)
    {
        ObjectInfo objectInfo = new()
        {
            position = pos,
            obj = o,
            objectType = ObjectType.Resource
        };

        return objectInfo;
    }

    #endregion
}
