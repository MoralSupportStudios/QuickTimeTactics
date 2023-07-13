using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEndState : BattleBaseState
{
    public override void EnterState(BattleController battleSystem)
    {
        Debug.Log("Battle End");
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
