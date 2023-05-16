using System;
using System.Collections.Generic;

public interface ITargetSelector
{
    public object Value { get; }
    public Type ValueType { get; }
    public bool IsDone { get; }
    public void Reset();
    public bool IsValid(object value, IReadOnlyCombatState state);
    public TargetSelectionResult ParseInput(ref Queue<string> args, IReadOnlyCombatState state);
    public enum TargetSelectionResult
    {
        INVALID, INCOMPLETE, FINISHED
    }
}