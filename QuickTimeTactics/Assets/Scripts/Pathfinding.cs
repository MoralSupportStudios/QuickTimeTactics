using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    private Node[,] nodeGrid;

    public Pathfinding(Node[,] nodeGrid)
    {
        this.nodeGrid = nodeGrid;
        CalculateAllNodeNeighbours();
    }

    public void CalculateAllNodeNeighbours()
    {
        //for square map
        int mapSizeX = nodeGrid.GetLength(0);
        int mapSizeY = nodeGrid.GetLength(1);
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                //check if node is on the edge of the map
                if (x > 0)
                {
                    nodeGrid[x, y].neighbours.Add(nodeGrid[x - 1, y]);
                }
                if (x < mapSizeX - 1)
                {
                    nodeGrid[x, y].neighbours.Add(nodeGrid[x + 1, y]);
                }
                if (y > 0)
                {
                    nodeGrid[x, y].neighbours.Add(nodeGrid[x, y - 1]);
                }
                if (y < mapSizeY - 1)
                {
                    nodeGrid[x, y].neighbours.Add(nodeGrid[x, y + 1]);
                }
            }
        }
    }

    public List<Node> GeneratePathTo(int sourceX, int sourceY, int destinationX, int destinationY)
    {
        Node sourceNode = nodeGrid[sourceX, sourceY];
        Node targetNode = nodeGrid[destinationX, destinationY];

        Dictionary<Node, float> nodeDistanceMap = new Dictionary<Node, float>();
        nodeDistanceMap[sourceNode] = 0;

        Dictionary<Node, Node> nodeBackwardsMap = new Dictionary<Node, Node>
        {
            [sourceNode] = null
        };

        List<Node> nodesToCheck = new List<Node>();
        for (int x = 0; x < nodeGrid.GetLength(0); x++)
        {
            for (int y = 0; y < nodeGrid.GetLength(1); y++)
            {
                Node currentNode = nodeGrid[x, y];
                if (currentNode != sourceNode)
                {
                    nodeDistanceMap[currentNode] = Mathf.Infinity;
                    nodeBackwardsMap[currentNode] = null;
                }
                nodesToCheck.Add(currentNode);
            }
        }

        while (nodesToCheck.Count > 0)
        {
            Node shortestDistanceNode = null;
            foreach (Node possibleNode in nodesToCheck)
            {
                if (shortestDistanceNode == null || nodeDistanceMap[possibleNode] < nodeDistanceMap[shortestDistanceNode])
                {
                    shortestDistanceNode = possibleNode;
                }
            }
            if (shortestDistanceNode == targetNode)
            {
                break;
            }
            nodesToCheck.Remove(shortestDistanceNode);
            foreach (Node neighbourNode in shortestDistanceNode.neighbours)
            {
                float distanceToNeighbour = nodeDistanceMap[shortestDistanceNode] + 1; // + neighbourNode.movementCost;
                if (distanceToNeighbour < nodeDistanceMap[neighbourNode])
                {
                    nodeDistanceMap[neighbourNode] = distanceToNeighbour;
                    nodeBackwardsMap[neighbourNode] = shortestDistanceNode;
                }
            }
        }

        if (nodeBackwardsMap[targetNode] == null)
        {
            // No path to the target
            return null;
        }

        List<Node> currentPath = new List<Node>();
        Node currentNodeOnPath = targetNode;
        while (currentNodeOnPath != null)
        {
            currentPath.Add(currentNodeOnPath);
            currentNodeOnPath = nodeBackwardsMap[currentNodeOnPath];
        }
        currentPath.Reverse();

        return currentPath;
    }

}
