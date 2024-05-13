using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviroManager : MonoBehaviour
{

    private BiomeGeneration biomeGeneration;
    private GrassSpawner grassSpawner;

    private void Awake()
    {
        biomeGeneration = GetComponent<BiomeGeneration>();
        grassSpawner = GetComponent<GrassSpawner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        biomeGeneration.Init();
        grassSpawner.Initial();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
