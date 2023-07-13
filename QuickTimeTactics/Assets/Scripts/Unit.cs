using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int tileX;
    public int tileY;
    public Node currentNode;
    public TileMap map;
    public bool isMoving;
    public List<Node> currentPathList;
    public List<Node> highlightedPath;

    public int maxHealth = 10;
    public int currentHealth = 10;
    public int attackPower = 2;
    public int speed = 5;

    public void MoveToTile(List<Node> path)
    {
        // Now we need to move the character along the path
        if (path != null && path.Count > 0)
        {
            StartCoroutine(MoveAlongPath(path));
        }
    }

    IEnumerator MoveAlongPath(List<Node> path)
    {
        isMoving = true;
        foreach (Node node in path)
        {
            yield return MoveToNode(node);

            ClearOldTile();
            SetNewTile(node);
        }
        isMoving = false;
        currentPathList = null;

        UnhighlightPath(path);
    }

    private IEnumerator MoveToNode(Node node)
    {
        GameObject tileObject = map.nodeGrid[node.x, node.y].tileObject;
        float combinedHeight = transform.localScale.y + tileObject.transform.localScale.y;
        Vector3 targetPosition = new Vector3(node.x, combinedHeight, node.y);

        while (new Vector3(transform.position.x, 0, transform.position.z) != new Vector3(targetPosition.x, 0, targetPosition.z))
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
            yield return null;
        }
    }

    private void ClearOldTile()
    {
        if (map.nodeGrid[tileX, tileY].tile.characterOnTile == gameObject)
        {
            map.nodeGrid[tileX, tileY].tile.characterOnTile = null;
        }
    }

    private void SetNewTile(Node node)
    {
        tileX = node.x;
        tileY = node.y;
        map.nodeGrid[tileX, tileY].tile.characterOnTile = gameObject;
    }

    private void UnhighlightPath(List<Node> path)
    {
        foreach (Node node in path)
        {
            node.tile.ChangeMaterial(map.normalMaterial);
        }
    }


    public void Attack(Unit other)
    {
        other.currentHealth -= attackPower;
    }
}
