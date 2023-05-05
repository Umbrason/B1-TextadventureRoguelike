using System;
using System.Collections.Generic;
using UnityEngine;

public struct CombatState : IBinarySerializable
{
    public Room Room { get; }
    public IReadOnlyDictionary<Guid, ICombatActor> CombatActors { get; }
    public IReadOnlyDictionary<Vector2Int, ITileModifier> TileModifiers { get; }
    public byte[] Bytes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    

    public CombatState(Room room, Dictionary<Guid, ICombatActor> combatActors, Dictionary<Vector2Int, ITileModifier> tileModifiers)
    {
        this.Room = room;
        this.CombatActors = combatActors;
        this.TileModifiers = tileModifiers;
    }
}