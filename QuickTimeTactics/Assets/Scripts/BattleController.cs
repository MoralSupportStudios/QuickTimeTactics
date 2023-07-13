using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public GameObject currentUnit;
    public TileMap tileMap;
    // Start is called before the first frame update
    void Start()
    {
        GameObject tileObject = tileMap.nodeGrid[0, 0].tileObject;
        float combinedHeight = currentUnit.transform.localScale.y + tileObject.transform.localScale.y;
        GameObject unitObject = Instantiate(currentUnit, new Vector3(0, combinedHeight, 0), Quaternion.identity);

        Unit unit = unitObject.GetComponent<Unit>();
        unit.tileX = 0;
        unit.tileY = 0;
        unit.map = tileMap;
        tileMap.nodeGrid[0, 0].occupyingObject = unitObject;
        tileMap.battleController = this;

        currentUnit = unitObject;
    }
}
