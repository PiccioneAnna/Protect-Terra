using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BiomeGeneration : MonoBehaviour
{

    #region Fields
    [Header("Tilemap References")]
    public Tilemap _tilemap_under;
    public Tilemap _tilemap0_dirt;
    public Tilemap _tilemap1_grass;
    public Tilemap _tilemap1_water;

    [Header("Tile References")]
    public TileBase dirt;
    public TileBase grass;
    public TileBase water;

    [Header("Stats")]
    public int width;
    public int height;
    public float fillPercent;
    public int liveNeighbors;
    public int startingStep;

    private int[,] dirtarray;
    private int[,] grassarray;
    private int[,] waterarray;

    private CellularAutomata cellularAutomata;

    #endregion

    // Start is called before the first frame update
    public void Init()
    {
        cellularAutomata = GetComponent<CellularAutomata>();
        cellularAutomata.Set(width, height, fillPercent, liveNeighbors, startingStep, 1);

        SetInitArrays();

        dirtarray = cellularAutomata.GenerateMap(null);
        waterarray = CreateInverse(waterarray, dirtarray);

        PopulateTilemaps();
        CreateBiomeTerrain();
    }

    public void CreateBiomeTerrain()
    {

    }

    public void PopulateTilemaps()
    {
        ClearTilemaps();
        PopulateUnderground();
        PopulateGrass();

        TilemapGenerator.PopulateTilemap(_tilemap0_dirt, dirtarray, dirt);
        TilemapGenerator.PopulateTilemap(_tilemap1_water, waterarray, water);
        TilemapGenerator.PopulateTilemap(_tilemap1_grass, grassarray, grass);

    }

    #region Helper Methods

    public void ClearTilemaps()
    {
        _tilemap_under.ClearAllTiles();
        _tilemap0_dirt.ClearAllTiles();
        _tilemap1_grass.ClearAllTiles();
        _tilemap1_water.ClearAllTiles();
    }

    public void PopulateUnderground()
    {
        int[,] temp = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                temp[x, y] = 1;
            }
        }

        TilemapGenerator.PopulateTilemap(_tilemap_under, temp, dirt);
    }

    public void PopulateGrass()
    {
        int[,] grassremoval = TilemapGenerator.DefineBorderPlacement(dirtarray);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (dirtarray[x, y] == 1 && grassremoval[x,y] != 1) 
                {
                    grassarray[x, y] = 1;
                }
            }
        }
    }

    public int[,] CreateInverse(int[,] newtm, int[,] reftm)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                newtm[x, y] = reftm[x, y] == 1 ? 0 : 1;

                if ((y-1 >= 0) && reftm[x,y] == 1 && reftm[x,y-1] == 0)
                {
                    newtm[x, y] = 1;
                }
            }
        }

        return newtm;
    }

    public void SetInitArrays()
    {
        dirtarray = new int[width, height];
        waterarray = new int[width, height];
        grassarray = new int[width, height];
    }

    #endregion
}
