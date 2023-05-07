using System;
using System.Collections.Generic;

public class CombatLog : IReadOnlyCombatLog
{
    private readonly Stack<CombatState> stateStack = new();
    public CombatState CurrentState => stateStack.Peek();
    public IEnumerable<CombatState> CombatStateHistory => stateStack;
    public event Action<CombatState> OnStateChanged;

    public CombatLog(CombatState initialState)
    {
        stateStack.Push(initialState);
    }

    public bool TryPerformAction(ICombatAction action)
    {
        if (!action.IsValid(CurrentState)) return false;
        var state = CurrentState;
        action.Apply(ref state);
        stateStack.Push(state);
        OnStateChanged?.Invoke(CurrentState);
        return true;
    }

}
public interface IReadOnlyCombatLog
{
    public IEnumerable<CombatState> CombatStateHistory { get; }
    CombatState CurrentState { get; }
}