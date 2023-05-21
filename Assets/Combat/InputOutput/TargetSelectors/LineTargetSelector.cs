using System;
using System.Collections.Generic;
using UnityEngine;

public class LineTargetSelector : ITargetSelector
{
    public LineTargetSelector(bool requiresLOS = false, bool requiresTileWalkable = false, int range = -1, int lineLength = -1, Func<IReadOnlyCombatState, Vector2Int, bool> validate = null)
    {
        Range = range;
        RequiresLOS = requiresLOS;
        RequiresTileWalkable = requiresTileWalkable;
        LineLength = lineLength;
        firstPositionSelector = new(requiresLOS, requiresTileWalkable, range);
        secondPositionSelector = new(requiresLOS, requiresTileWalkable, range, ValidateLine);
    }
    private RangedPositionSelector firstPositionSelector;
    private RangedPositionSelector secondPositionSelector;
    public int Range { get; }
    public bool RequiresLOS { get; }
    public bool RequiresTileWalkable { get; }
    public int LineLength { get; }

    public object Value => new Vector2Int[] { (Vector2Int)firstPositionSelector.Value, (Vector2Int)secondPositionSelector.Value };
    public Type ValueType => typeof(Vector2Int[]);
    public bool IsDone => firstPositionSelector.IsDone && secondPositionSelector.IsDone;

    private bool ValidateLine(IReadOnlyCombatState state, Vector2Int pos)
    {
        if ((firstPositionSelector.Position - pos).sqrMagnitude > LineLength * LineLength && LineLength > 0)
        {
            ConsoleOutput.Println("Positions too far apart!");
            return false;
        }
        return true;
    }

    public void Reset()
    {
        firstPositionSelector.Reset();
        secondPositionSelector.Reset();
    }

    public bool IsValid(object value, IReadOnlyCombatState state)
    {
        var positions = (Vector2Int[])value;
        return positions.Length == 2 && (positions[0] - positions[1]).sqrMagnitude <= LineLength * LineLength;
    }

    public ITargetSelector.TargetSelectionResult ParseInput(ref Queue<string> args, IReadOnlyCombatState state)
    {
        ITargetSelector.TargetSelectionResult result;
        if (!firstPositionSelector.IsDone && (result = firstPositionSelector.ParseInput(ref args, state)) != ITargetSelector.TargetSelectionResult.FINISHED) return result;
        if (!secondPositionSelector.IsDone && (result = secondPositionSelector.ParseInput(ref args, state)) != ITargetSelector.TargetSelectionResult.FINISHED) return result;
        return ITargetSelector.TargetSelectionResult.FINISHED;
    }
}