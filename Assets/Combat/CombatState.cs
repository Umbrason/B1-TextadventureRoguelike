using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PackageSystem;
using UnityEngine;

public struct CombatState : IBinarySerializable
{
    public Room Room { get; set; }
    public IReadOnlyDictionary<Guid, ICombatActor> CombatActors { get; private set; }
    public IReadOnlyDictionary<Vector2Int, Guid> ActorPositions { get; private set; }
    public IReadOnlyDictionary<Vector2Int, ITileModifier> TileModifiers { get; private set; }
    public Queue<Guid> TurnQueue;
    public Guid? ActiveActorGuid { get; set; }
    public ICombatActor ActiveActor => ActiveActorGuid.HasValue ? CombatActors[ActiveActorGuid.Value] : null;

    public byte[] ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteIBinarySerializable(Room);
            stream.WriteDictionary(CombatActors, stream.WriteGuid, stream.WriteIBinarySerializable);
            stream.WriteDictionary(ActorPositions, stream.WriteVector2Int, stream.WriteGuid);
            stream.WriteDictionary(TileModifiers, stream.WriteVector2Int, stream.WriteIBinarySerializableData);
            stream.WriteEnumerable(TurnQueue, stream.WriteGuid);
            stream.WriteGuid(ActiveActorGuid ?? Guid.Empty);
            return stream.GetAllBytes();
        }
        set
        {
            var stream = new MemoryStream(value);
            this.Room = stream.ReadIBinarySerializable<Room>();
            CombatActors = stream.ReadDictionary(stream.ReadGuid, stream.ReadIBinarySerializable<ICombatActor>);
            ActorPositions = stream.ReadDictionary(stream.ReadVector2Int, stream.ReadGuid);
            TileModifiers = stream.ReadDictionary(stream.ReadVector2Int, stream.ReadIBinarySerializable<ITileModifier>);
            TurnQueue = new(stream.ReadEnumerable(stream.ReadGuid));
            var guid = stream.ReadGuid();
            ActiveActorGuid = guid == Guid.Empty ? null : guid;;
        }
    }

    public CombatState DeepCopy()
    {
        var copy = new CombatState();
        copy.ByteData = this.ByteData;
        return copy;
    }

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
        return this.WithModifiedActor(actor);
    }

    public CombatState WithModifiedActor(ICombatActor actor)
    {
        var modifiedActors = new Dictionary<Guid, ICombatActor>(CombatActors);
        modifiedActors[actor.Guid] = actor;
        return this.WithActors(modifiedActors);
    }

    public CombatState WithTileModifiers(Dictionary<Vector2Int, ITileModifier> tileModifiers) => new(Room, CombatActors, tileModifiers, TurnQueue, ActiveActorGuid);
    public CombatState WithActors(Dictionary<Guid, ICombatActor> actors) => new(Room, actors, TileModifiers, TurnQueue, ActiveActorGuid);
    public CombatState WithTurnQueue(Queue<Guid> turnQueue) => new(Room, CombatActors, TileModifiers, turnQueue, ActiveActorGuid);
    public CombatState WithActiveActor(Guid? activeActorGuid) => new(Room, CombatActors, TileModifiers, TurnQueue, activeActorGuid);

    private void BuildTurnQueue()
    {
        var teamDict = new SortedList<int, SortedList<int, ICombatActor>>();
        foreach (var actor in CombatActors.Values)
        {
            var bucket = teamDict.ContainsKey(actor.Alignment) ? teamDict[actor.Alignment] : teamDict[actor.Alignment] = new();
            bucket.Add(int.MaxValue - actor.Initiative, actor);
        }
        var alignments = teamDict.Keys.OrderBy(alignment => teamDict[alignment].Keys[0]).ToArray();
        int teamIndex = 0;
        while (TurnQueue.Count < CombatActors.Count)
        {
            var bucket = teamDict[alignments[teamIndex]];
            if (bucket.Count > 0)
            {
                TurnQueue.Enqueue(bucket.Values[bucket.Count - 1].Guid);
                bucket.RemoveAt(bucket.Count - 1);
            }
            teamIndex = ++teamIndex % alignments.Length;
        }
    }

    public CombatState(Room room, IReadOnlyDictionary<Guid, ICombatActor> combatActors, IReadOnlyDictionary<Vector2Int, ITileModifier> tileModifiers, Queue<Guid> turnQueue, Guid? activeActorGuid)
    {
        this.Room = room;
        this.CombatActors = combatActors;
        this.ActorPositions = new Dictionary<Vector2Int, Guid>(combatActors.Select(pair => new KeyValuePair<Vector2Int, Guid>(pair.Value.Position, pair.Key)));
        this.TileModifiers = tileModifiers;
        this.TurnQueue = turnQueue;
        this.ActiveActorGuid = activeActorGuid;
    }
}