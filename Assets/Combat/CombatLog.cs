using System.Collections.Generic;

public class CombatLog
{
    private readonly Stack<CombatState> stateStack = new();
    private CombatState CurrentState => stateStack.Peek();
    
    void RecordAction(ICombatAction action)
    {
        stateStack.Push(action.Apply(CurrentState));
    }
}