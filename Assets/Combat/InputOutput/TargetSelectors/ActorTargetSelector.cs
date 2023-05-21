using System;
using UnityEngine;

public class ActorTargetSelector : RangedPositionSelector
{
    public ActorTargetSelector(bool requiresLOS = false, int range = -1, Func<IReadOnlyCombatState, Vector2Int, bool> validate = null) : base(requiresLOS, false, range, (state, pos) => ActorTargetSelector.Validate(pos, state, requiresLOS, range)) { }

    private static bool Validate(Vector2Int position, IReadOnlyCombatState state, bool requiresLOS, int range)
    {
        if (!state.ActorPositions.ContainsKey(position)) { ConsoleOutput.Println("No actor at target location!"); return false; }
        return true;
    }
}