using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTransitionState : BattleBaseState
{
    public override void EnterState(BattleController battleSystem)
    {
        Debug.Log("Battle Transition");
        // Determine whose turn it is:
        WhosNext(battleSystem);
        CheckIfGameIsOver(battleSystem);
    }

    private static void WhosNext(BattleController battleSystem)
    {
        if (battleSystem.currentUnit.CompareTag("Player"))
        {
            // If the current unit is a player, go to player state:
            battleSystem.TransitionToState(battleSystem.playerState);
        }
        else
        {
            // If the current unit is an enemy, go to enemy state:
            battleSystem.TransitionToState(battleSystem.enemyState);
        }
    }

    private static void CheckIfGameIsOver(BattleController battleSystem)
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            // If there are no enemy units left, player wins
            battleSystem.TransitionToState(battleSystem.endState);
            // Do other stuff for when the player wins...
        }
        else if (GameObject.FindGameObjectsWithTag("Player").Length == 0)
        {
            // If there are no player units left, player loses
            battleSystem.TransitionToState(battleSystem.endState);
            // Do other stuff for when the player loses...
        }
    }

    public override void Update(BattleController battleSystem)
    {
        //throw new System.NotImplementedException();
    }
    public override void ExitState(BattleController battleSystem)
    {
        //throw new System.NotImplementedException();
    }
}
