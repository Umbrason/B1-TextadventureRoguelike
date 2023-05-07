using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct CombatState : IBinarySerializable
{
    public Room Room { get; }
    public IReadOnlyDictionary<Guid, ICombatActor> CombatActors { get; }
    public IReadOnlyDictionary<Vector2Int, Guid> ActorPositions { get; }
    public IReadOnlyDictionary<Vector2Int, ITileModifier> TileModifiers { get; }
    public byte[] Bytes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool IsTileWalkable(Vector2Int tile)
    {
        if (Room[tile] != Room.Tile.FLOOR) return false;
        if (ActorPositions.ContainsKey(tile)) return false;
        if (TileModifiers.TryGetValue(tile, out var modifier) && modifier.BlocksMovement) return false;
        return true;
    }

    

    public CombatState MoveActor(Guid actorGuid, Vector2Int targetLocation)
    {
        var actor = this.CombatActors[actorGuid];
        actor.Position = targetLocation;
        return this.WithModifiedActor(actor);
    }

    public CombatState WithModifiedActor(ICombatActor actor)
    {
        var modifiedActors = new Dictionary<Guid, ICombatActor>(CombatActors);
        modifiedActors[actor.Guid] = actor;
        return this.WithActors(modifiedActors);
    }

    public CombatState WithTileModifiers(Dictionary<Vector2Int, ITileModifier> tileModifiers) => new(Room, CombatActors, tileModifiers);
    public CombatState WithActors(Dictionary<Guid, ICombatActor> actors) => new(Room, actors, TileModifiers);

    public CombatState(Room room, IReadOnlyDictionary<Guid, ICombatActor> combatActors, IReadOnlyDictionary<Vector2Int, ITileModifier> tileModifiers)
    {
        this.Room = room;
        this.CombatActors = combatActors;
        this.ActorPositions = new Dictionary<Vector2Int, Guid>(combatActors.Select(pair => new KeyValuePair<Vector2Int, Guid>(pair.Value.Position, pair.Key)));
        this.TileModifiers = tileModifiers;
    }
}