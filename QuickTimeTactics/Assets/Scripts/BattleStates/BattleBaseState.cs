public abstract class BattleBaseState
{
    public abstract void EnterState(BattleController battleSystem);
    public abstract void Update(BattleController battleSystem);
    public abstract void ExitState(BattleController battleSystem);
}