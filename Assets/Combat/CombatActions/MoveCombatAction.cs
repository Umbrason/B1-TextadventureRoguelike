
using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveCombatAction : ICombatAction
{
    public Guid targetActor;
    public Vector2Int targetLocation;

    public MoveCombatAction(Guid targetActor, Vector2Int targetLocation)
    {
        this.targetActor = targetActor;
        this.targetLocation = targetLocation;
    }

    public void Apply(ref CombatState state)
    {

    }

    bool ICombatAction.IsValid(CombatState state)
    {
        //pathfinding here?
        return state.IsTileWalkable(targetLocation);
    }
}
