using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    private const int FlatTileTypeIndex = 0;
    private const int ElevatedTileTypeIndex = 1;

    public BattleController battleController;
    public TileType[] tileTypes;
    public int[,] tileGrid;
    public Node[,] nodeGrid;
    public readonly int mapSizeX = 5;
    public readonly int mapSizeY = 5;
    public Pathfinding pathfinding;
    public Material normalMaterial;
    public Material highlightedMaterial;


    // Start is called before the first frame update
    void Start()
    {
        GenerateTileGridData();
        GenerateNodeGridPathfinding();
        // Now that all the nodes exist, calculate their neighbours
        pathfinding = new Pathfinding(nodeGrid); // We initialize it with our node grid

        GenerateVisualRepresentationOfMap();
    }

    public void GenerateTileGridData()
    {
        tileGrid = new int[mapSizeX, mapSizeY];
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                if (x == mapSizeX / 2 && y == mapSizeY / 2)  // If we are at the center of the map
                {
                    tileGrid[x, y] = ElevatedTileTypeIndex;
                }
                else
                {
                    tileGrid[x, y] = FlatTileTypeIndex;
                }
            }
        }
    }


    public void GenerateNodeGridPathfinding()
    {
        nodeGrid = new Node[mapSizeX, mapSizeY];
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                nodeGrid[x, y] = new Node();
                nodeGrid[x, y].x = x;
                nodeGrid[x, y].y = y;
            }
        }
    }

    public void GenerateVisualRepresentationOfMap()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                TileType currentTileType = tileTypes[tileGrid[x, y]];
                GameObject newTileObject = Instantiate(currentTileType.tileVisualPrefab, new Vector3(x, currentTileType.elevation, y), Quaternion.identity);
                nodeGrid[x, y].tileObject = newTileObject;
                newTileObject.transform.parent = transform;
                newTileObject.name = $"Tile_{x}_{y}";
                SelectableTile selectableTile = newTileObject.GetComponent<SelectableTile>();
                selectableTile.tileX = x;
                selectableTile.tileY = y;
                selectableTile.map = this;
                selectableTile.battleController = battleController;
                nodeGrid[x, y].tile = selectableTile;
                nodeGrid[x, y].elevation = currentTileType.elevation;
            }
        }
    }

}
