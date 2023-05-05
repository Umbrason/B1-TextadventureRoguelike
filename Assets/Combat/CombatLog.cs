using System;
using System.Collections.Generic;

public class CombatLog
{
    private readonly Stack<CombatState> stateStack = new();
    private CombatState CurrentState => stateStack.Peek();

    public event Action<CombatState> OnStateChanged;

    void PerformAction(ICombatAction action)
    {
        var state = CurrentState;
        action.Apply(ref state);
        stateStack.Push(state);
    }
}