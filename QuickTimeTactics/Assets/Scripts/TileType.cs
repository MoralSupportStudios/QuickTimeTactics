using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileType : MonoBehaviour
{
    public string tileName;
    public GameObject tileVisualPrefab;

    public bool isWalkable = true;
    public float movementCost = 1;
    public float elevation = 1;
}
