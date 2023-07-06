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

    public int maxHealth = 10;
    public int currentHealth = 10;
    public int attackPower = 2;
    public int speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
