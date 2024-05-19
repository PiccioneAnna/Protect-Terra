using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnviroManager : MonoBehaviour
{
    public enum ObjectType
    {
        Grass,
        Resource,
        PlacedObject
    }

    public struct ObjectInfo
    {
        public Vector2 position;
        public GameObject obj;
        public ObjectType objectType;
    }

    [HideInInspector] public BiomeGeneration biomeGeneration;
    [HideInInspector] public GrassSpawner grassSpawner;
    [HideInInspector] public EnviroSpawner enviroSpawner;

    public List<ObjectInfo> spawnedObjects;

    public int layer = 2;

    private void Awake()
    {
        biomeGeneration = GetComponent<BiomeGeneration>();
        grassSpawner = GetComponent<GrassSpawner>();
        enviroSpawner = GetComponent<EnviroSpawner>();       
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.enviroManager = this;
        spawnedObjects = new List<ObjectInfo>();

        biomeGeneration.Init();
        grassSpawner.Initial();
        enviroSpawner.Init();
    }

    public bool CheckObjectPosition(Vector2 pos)
    {
        if(spawnedObjects.Any(x => x.position == pos)) { return true; }
        return false;
    }

    public void RemoveAtPos(Vector2 pos)
    {
        if(spawnedObjects.Any(x => x.position == pos))
        {
            var obj = spawnedObjects.Find(x => x.position == pos);
            spawnedObjects.Remove(obj);
        }
    }
}
