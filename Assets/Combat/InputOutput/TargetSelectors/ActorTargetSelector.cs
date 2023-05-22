using System;
using UnityEngine;

public class ActorTargetSelector : RangedPositionSelector
{
    public ActorTargetSelector(bool requiresLOS = false, int range = -1, bool allowSelf = false, Func<IReadOnlyCombatState, Vector2Int, bool> validate = null) : base(requiresLOS, false, range, (state, pos) => ActorTargetSelector.Validate(pos, state, requiresLOS, allowSelf, range)) { }

    private static bool Validate(Vector2Int position, IReadOnlyCombatState state, bool requiresLOS, bool allowSelf, int range)
    {
        if (!state.ActorPositions.ContainsKey(position)) { ConsoleOutput.Println("No actor at target location!"); return false; }
        if(!allowSelf && state.ActorPositions[position] == state.ActiveActorGuid) { ConsoleOutput.Println("Cannot hit self!"); return false; }
        return true;
    }
}