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
    public EnviroSpawner enviroSpawner;

    [Header("Global")]
    public GameObject itemVisual; // for always having item infront of all UI
    public TilemapInfoManager tilemapInfoManager;

    // Hidden : set within script

    [HideInInspector] public static GameManager Instance;
    [HideInInspector] public Reader reader;
    [HideInInspector] public Inventory.Manager inventory;
    [HideInInspector] public TilemapScripts.CropsManager cropsManager;
    [HideInInspector] public TimeController timeController;
    [HideInInspector] public SceneManager sceneManager;
    [HideInInspector] public OnScreenMessageSystem screenMessageSystem;
    [HideInInspector] public PlaceableObjectsReferenceManager placeableObjectsManager;

    #endregion

    #region Runtime
    /// <summary>
    /// Class is a singleton, only one should exist at ALL times
    /// </summary>
    void Awake()
    {
        //if (Instance == null) // If there is no instance already
        //{
        //    Instance = this;
        //}
        //else if (Instance != this) // If there is already an instance and it's not `this` instance
        //{
        //    Destroy(gameObject); // Destroy the GameObject, this component is attached to
        //}

        //inventory = GetComponent<Inventory.Manager>();
        //reader = GetComponent<Reader>();
        //cropsManager = GetComponent<TilemapScripts.CropsManager>();
        //timeController = GetComponent<TimeController>();
        //sceneManager = GetComponent<SceneManager>();
        //screenMessageSystem = GetComponent<OnScreenMessageSystem>();
        //placeableObjectsManager = GetComponent<PlaceableObjectsReferenceManager>();
    
    }

    private void Start()
    {

    }

    #endregion

    #region Helpers

    public void FindTilemaps()
    {
        GameObject grid = GameObject.Find("Grid Base");

        if (grid == null) { return; }

        tilemapInfoManager = grid.GetComponent<TilemapInfoManager>();
    }

    public void FindEnviroSpawner()
    {
        enviroSpawner = FindAnyObjectByType<EnviroSpawner>();
    }

    #endregion
}
