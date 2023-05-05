
using System;
using UnityEngine;

public class MoveCombatAction : ICombatAction
{
    public Guid targetActor;
    public Vector2Int targetPosition;

    public void Apply(ref CombatState state)
    {
        var actor = state.CombatActors[targetActor];
        actor.Position = targetPosition;
    }
}
