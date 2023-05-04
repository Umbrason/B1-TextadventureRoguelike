using System;

public struct TurnInfo
{
    private Action EndTurnAction;
    public CombatLog Log { get; }
    public void EndTurn() => EndTurnAction?.Invoke();
    public TurnInfo(Action EndTurnAction, CombatLog Log)
    {
        this.EndTurnAction = EndTurnAction;
        this.Log = Log;
    }
}