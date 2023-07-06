using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public BattleController battleController;
    public TileType[] tileTypes;
    int[,] tileGrid;
    public Node[,] nodeGrid;
    public readonly int mapSizeX = 5;
    public readonly int mapSizeY = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateTileGridData();
        GenerateNodeGridPathfinding();
        GenerateVisualRepresentationOfMap();
    }    

    public void GenerateTileGridData()
    {
        tileGrid = new int[mapSizeX, mapSizeY];
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                tileGrid[x, y] = 0;
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
        // Now that all the nodes exist, calculate their neighbours
        CalculateAllNodeNeighbours();
    }

    public void CalculateAllNodeNeighbours()
    {
        //for square map
        for(int x = 0; x < mapSizeX; x++)
        {
            for(int y = 0; y < mapSizeY; y++)
            {
                //check if node is on the edge of the map
                if(x > 0)
                {
                    nodeGrid[x, y].neighbours.Add(nodeGrid[x - 1, y]);
                }
                if(x < mapSizeX - 1)
                {
                    nodeGrid[x, y].neighbours.Add(nodeGrid[x + 1, y]);
                }
                if(y > 0)
                {
                    nodeGrid[x, y].neighbours.Add(nodeGrid[x, y - 1]);
                }
                if(y < mapSizeY - 1)
                {
                    nodeGrid[x, y].neighbours.Add(nodeGrid[x, y + 1]);
                }
            }
        }
    }
    public void GenerateVisualRepresentationOfMap()
    {
        for(int x = 0; x < mapSizeX; x++)
        {
            for(int y = 0; y < mapSizeY; y++)
            {
                TileType currentTileType = tileTypes[tileGrid[x, y]];
                GameObject newTileObject = Instantiate(currentTileType.tileVisualPrefab, new Vector3(x, 0, y), Quaternion.identity);
                newTileObject.transform.parent = transform;
                newTileObject.name = $"Tile_{x}_{y}";
                SelectableTile selectableTile = newTileObject.GetComponent<SelectableTile>();
                selectableTile.tileX = x;
                selectableTile.tileY = y;
                selectableTile.map = this;
                selectableTile.battleController = battleController;
                nodeGrid[x, y].tile = selectableTile;
            }
        }
    }

    public void GeneratePathTo(int destinationX, int destinationY)
    {
        // Clear out our unit's old path.
        Unit currentUnit = battleController.currentUnit.GetComponent<Unit>();
        currentUnit.currentPathList = null;

        Dictionary<Node, float> nodeDistanceMap = new Dictionary<Node, float>();
        Node sourceNode = nodeGrid[currentUnit.tileX, currentUnit.tileY];
        nodeDistanceMap[sourceNode] = 0;
        Dictionary<Node, Node> nodeBackwardsMap = new()
        {
            [sourceNode] = null
        };
        
        List<Node> nodesToCheck = new List<Node>();
        for(int x = 0; x < mapSizeX; x++)
        {
            for(int y = 0; y < mapSizeY; y++)
            {
                Node currentNode = nodeGrid[x, y];
                if(currentNode != sourceNode)
                {
                    nodeDistanceMap[currentNode] = Mathf.Infinity;
                    nodeBackwardsMap[currentNode] = null;
                }
                nodesToCheck.Add(currentNode);
            }
        }
        Node targetNode = nodeGrid[destinationX, destinationY];
        while (nodesToCheck.Count > 0)
        {
            Node shortestDistanceNode = null;
            foreach(Node possibleNode in nodesToCheck)
            {
                if(shortestDistanceNode == null || nodeDistanceMap[possibleNode] < nodeDistanceMap[shortestDistanceNode])
                {
                    shortestDistanceNode = possibleNode;
                }
            }
            if(shortestDistanceNode == targetNode)
            {
                break;
            }
            nodesToCheck.Remove(shortestDistanceNode);
            foreach(Node neighbourNode in shortestDistanceNode.neighbours)
            {
                float distanceToNeighbour = nodeDistanceMap[shortestDistanceNode];// + neighbourNode.movementCost;
                if(distanceToNeighbour < nodeDistanceMap[neighbourNode])
                {
                    nodeDistanceMap[neighbourNode] = distanceToNeighbour;
                    nodeBackwardsMap[neighbourNode] = shortestDistanceNode;
                }
            }
        }
        if (nodeBackwardsMap[targetNode] == null)
        {
            // No path to the target
            return;
        }
        List<Node> currentPath = new List<Node>();
        Node currentNodeOnPath = targetNode;
        while (currentNodeOnPath != null)
        {
            currentPath.Add(currentNodeOnPath);
            currentNodeOnPath = nodeBackwardsMap[currentNodeOnPath];
        }
        currentPath.Reverse();

        currentUnit.currentPathList = currentPath;
    }
}
