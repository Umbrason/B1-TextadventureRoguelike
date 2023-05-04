using System;
using System.Collections.Generic;
using UnityEngine;

public struct CombatState
{
    public readonly IReadOnlyDictionary<Guid, ICombatActor> combatActors;
    public readonly IReadOnlyDictionary<Vector2Int, ITileModifier> tileModifiers;
}