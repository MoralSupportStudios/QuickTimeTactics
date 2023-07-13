using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SelectableTile : MonoBehaviour
{
    public int tileX;
    public int tileY;
    public TileMap map;
    public BattleController battleController;
    public GameObject characterOnTile;

    void OnMouseDown()
    {
        //if (battleController.currentState is not BattlePlayerState)
        //{
        //    return;
        //}

        Unit currentCharacter = battleController.currentUnit.GetComponent<Unit>();
        if (currentCharacter.isMoving)
        {
            return;
        }

        List<Node> path = map.pathfinding.GeneratePathTo(currentCharacter.tileX, currentCharacter.tileY, tileX, tileY);
        currentCharacter.currentPathList = path;
        currentCharacter.highlightedPath = null;
        currentCharacter.MoveToTile(path);
    }

    void OnMouseEnter()
    {
        //if (battleController.currentState is not BattlePlayerState)
        //    return;

        Unit currentCharacter = battleController.currentUnit.GetComponent<Unit>();
        if (currentCharacter.isMoving)
        {
            return;
        }
        // Clear old highlighted path
        if (currentCharacter.highlightedPath != null)
        {
            foreach (Node node in currentCharacter.highlightedPath)
            {
                node.tile.ChangeMaterial(map.normalMaterial);
            }
        }

        // Generate and highlight new path
        currentCharacter.highlightedPath = map.pathfinding.GeneratePathTo(currentCharacter.tileX, currentCharacter.tileY, tileX, tileY);
        if (currentCharacter.highlightedPath != null)
        {
            foreach (Node node in currentCharacter.highlightedPath)
            {
                node.tile.ChangeMaterial(map.highlightedMaterial);
            }
        }
    }

    void OnMouseExit()
    {
        //if (battleController.currentState is not BattlePlayerState)
        //    return;

        Unit currentCharacter = battleController.currentUnit.GetComponent<Unit>();
        if (currentCharacter.isMoving)
        {
            return;
        }
        // Clear highlighted path
        if (currentCharacter.highlightedPath != null)
        {
            foreach (Node node in currentCharacter.highlightedPath)
            {
                node.tile.ChangeMaterial(map.normalMaterial);
            }
        }
        currentCharacter.highlightedPath = null;
    }


    public void ChangeMaterial(Material newMaterial)
    {
        GetComponent<MeshRenderer>().material = newMaterial;
    }
}
