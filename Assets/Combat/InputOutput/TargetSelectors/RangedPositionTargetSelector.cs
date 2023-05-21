using System;
using UnityEngine;

public class RangedPositionSelector : PositionTargetSelector
{
    public RangedPositionSelector(bool requiresLOS = false, bool requiresTileWalkable = false, int range = -1, Func<IReadOnlyCombatState, Vector2Int, bool> validate = null) : base((position, state) => Validate(position, state, requiresLOS, requiresTileWalkable, range) && (validate?.Invoke(state, position) ?? true)) { 
        Range = range;
        RequiresLOS = requiresLOS;
        RequiresTileWalkable = requiresTileWalkable;
    }

    public int Range {get;}
    public bool RequiresLOS {get;}
    public bool RequiresTileWalkable {get;}

    private static bool Validate(Vector2Int position, IReadOnlyCombatState state, bool requiresLOS, bool requiresTileWalkable, int range)
    {        
        var activeActor = state.ActiveActor;
        var delta = position - activeActor.Position;

        if (range >= 0 && range * range < delta.x * delta.x + delta.y * delta.y) { ConsoleOutput.Println("Out of range!");               return false; }
        if (requiresTileWalkable && !state.IsTileWalkable(position))             { ConsoleOutput.Println("Destination is blocked!");     return false; }
        if (requiresLOS && !state.IsLOSClear(activeActor.Position, position))    { ConsoleOutput.Println("Cannot see target location!"); return false; }
        return true;
    }
}