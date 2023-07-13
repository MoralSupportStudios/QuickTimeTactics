using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStartState : BattleBaseState
{
    public override void EnterState(BattleController battleSystem)
    {
        Debug.Log("Battle Start"); 
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
