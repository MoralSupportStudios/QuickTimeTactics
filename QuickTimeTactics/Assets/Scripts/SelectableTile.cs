using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableTile : MonoBehaviour
{
    public int tileX;
    public int tileY;
    public Material normalMaterial;
    public Material highlightedMaterial;
    public TileMap map;
    public BattleController battleController;
}
