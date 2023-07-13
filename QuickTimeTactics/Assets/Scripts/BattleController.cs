using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public GameObject currentUnit;
    public TileMap tileMap;

    public List<GameObject> playerUnits = new List<GameObject>();
    public List<GameObject> enemyUnits = new List<GameObject>();
    public List<GameObject> allUnits;
    public List<GameObject> turnOrder;
    public int currentUnitIndex;

    public BattleBaseState currentState;
    public readonly BattleStartState startState = new BattleStartState();
    public readonly BattlePlayerState playerState = new BattlePlayerState();
    public readonly BattleEnemyState enemyState = new BattleEnemyState();
    public readonly BattleTransitionState transitionState = new BattleTransitionState();
    public readonly BattleEndState endState = new BattleEndState();
    private void Start()
    {
        tileMap.battleController = this;

        InstantiateUnits(playerUnits, 0);
        InstantiateUnits(enemyUnits, tileMap.mapSizeY - 1);

        CalculateTurnOrder();

        TransitionToState(startState);
    }

    private void InstantiateUnits(List<GameObject> units, int yPos)
    {
        for (int i = 0; i < units.Count; i++)
        {
            GameObject unitPrefab = units[i];
            GameObject tileObject = tileMap.nodeGrid[i, yPos].tileObject;
            float combinedHeight = unitPrefab.transform.localScale.y / 2 + tileObject.transform.localScale.y / 2;
            GameObject unitObject = Instantiate(unitPrefab, new Vector3(i, combinedHeight, yPos), Quaternion.identity);
            Unit unit = unitObject.GetComponent<Unit>();
            unit.tileX = i;
            unit.tileY = yPos;
            unit.map = tileMap;
            tileMap.nodeGrid[i, yPos].occupyingObject = unitObject;
            units[i] = unitObject; // replace the prefab with the instantiated unit

            allUnits.Add(unitObject);
        }
    }
    // This method sorts the allUnits list based on speed and assigns the result to turnOrder:
    private void CalculateTurnOrder()
    {
        turnOrder = new List<GameObject>(allUnits);
        turnOrder.Sort((unit1, unit2) => unit2.GetComponent<Unit>().speed.CompareTo(unit1.GetComponent<Unit>().speed));
        currentUnitIndex = 0;
        currentUnit = turnOrder[currentUnitIndex];
    }

    // Call this method whenever a unit ends its turn:
    public void EndTurn()
    {
        currentUnitIndex++;
        if (currentUnitIndex >= turnOrder.Count)
        {
            // All units have had their turn, so recalculate the turn order:
            CalculateTurnOrder();
        }
    }

    private void Update()
    {
        currentState.Update(this);
    }

    public void TransitionToState(BattleBaseState state)
    {
        currentState = state;
        Debug.Log("Transitioning to " + currentState);
        currentState.EnterState(this);
    }

}
