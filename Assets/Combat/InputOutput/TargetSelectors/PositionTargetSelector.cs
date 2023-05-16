using System;
using System.Collections.Generic;
using UnityEngine;

public class PositionTargetSelector : ITargetSelector
{
    private readonly List<int> posArgs = new();
    public object Value => Position;
    public Type ValueType => typeof(Vector2Int);
    public Vector2Int Position => IsDone ? new Vector2Int(posArgs[0], posArgs[1]) : default;
    public bool IsDone => posArgs.Count == 2;
    private Func<Vector2Int, IReadOnlyCombatState, bool> validate;
    public PositionTargetSelector(Func<Vector2Int, IReadOnlyCombatState, bool> validate) => this.validate = validate;
    public PositionTargetSelector() => this.validate = (_, _) => true;

    public bool IsValid(object value, IReadOnlyCombatState state) => this.validate.Invoke((Vector2Int)value, state);
    public ITargetSelector.TargetSelectionResult ParseInput(ref Queue<string> args, IReadOnlyCombatState state)
    {
        while (args.Count > 0 && posArgs.Count < 2)
        {
            var str = args.Dequeue();
            if (!int.TryParse(str, out int p))
            {
                ConsoleOutput.Println($"Invalid argument: '{str}' is not a number!");
                return ITargetSelector.TargetSelectionResult.INVALID;
            }
            posArgs.Add(p);
        }
        if (posArgs.Count == 0) ConsoleOutput.Println($"Enter target x y:");
        if (posArgs.Count == 1) ConsoleOutput.Println($"Enter target y:");
        if (IsDone && !IsValid(Position, state)) return ITargetSelector.TargetSelectionResult.INVALID;
        return IsDone ? ITargetSelector.TargetSelectionResult.FINISHED : ITargetSelector.TargetSelectionResult.INCOMPLETE;
    }
    public void Reset() => posArgs.Clear();
}