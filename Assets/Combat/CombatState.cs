using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CombatState : IReadOnlyCombatState, IBinarySerializable
{
    public RoomInfo Room { get; set; }
    public Dictionary<Guid, ICombatActor> CombatActors { get; private set; }
    public Dictionary<Vector2Int, Guid> ActorPositions { get; private set; }
    public Dictionary<Vector2Int, ITileModifier> TileModifiers { get; private set; }
    public Queue<Guid> TurnQueue;
    public bool ActiveActorStunned { get; set; }
    public Guid? ActiveActorGuid { get; set; }
    public ICombatActor ActiveActor => ActiveActorGuid.HasValue ? CombatActors[ActiveActorGuid.Value] : null;

    IReadOnlyDictionary<Guid, IReadOnlyCombatActor> IReadOnlyCombatState.CombatActors => CombatActors.Values.Select(actor => (IReadOnlyCombatActor)actor).ToDictionary(actor => actor.Guid);
    IReadOnlyCollection<Guid> IReadOnlyCombatState.TurnQueue => TurnQueue;
    IReadOnlyDictionary<Vector2Int, Guid> IReadOnlyCombatState.ActorPositions => ActorPositions;
    IReadOnlyDictionary<Vector2Int, ITileModifier> IReadOnlyCombatState.TileModifiers => TileModifiers;

    public byte[] ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteIBinarySerializable(Room);
            stream.WriteDictionary(CombatActors, stream.WriteGuid, stream.WriteIBinarySerializable);
            stream.WriteDictionary(ActorPositions, stream.WriteVector2Int, stream.WriteGuid);
            stream.WriteDictionary(TileModifiers, stream.WriteVector2Int, stream.WriteIBinarySerializable);
            stream.WriteEnumerable(TurnQueue, stream.WriteGuid);
            stream.WriteGuid(ActiveActorGuid ?? Guid.Empty);
            return stream.GetAllBytes();
        }
        set
        {
            var stream = new MemoryStream(value);
            this.Room = stream.ReadIBinarySerializable<RoomInfo>();
            CombatActors = stream.ReadDictionary(stream.ReadGuid, stream.ReadIBinarySerializable<ICombatActor>);
            ActorPositions = stream.ReadDictionary(stream.ReadVector2Int, stream.ReadGuid);
            TileModifiers = stream.ReadDictionary(stream.ReadVector2Int, stream.ReadIBinarySerializable<ITileModifier>);
            TurnQueue = new(stream.ReadEnumerable(stream.ReadGuid));
            var guid = stream.ReadGuid();
            ActiveActorGuid = guid == Guid.Empty ? null : guid; ;
        }
    }

    private CombatState() { }
    public CombatState DeepCopy()
    {
        var copy = new CombatState();
        copy.ByteData = this.ByteData;
        return copy;
    }

    public bool IsTileWalkable(Vector2Int tile)
    {
        if (Room[tile] != RoomInfo.Tile.FLOOR) return false;
        if (ActorPositions.ContainsKey(tile)) return false;
        if (TileModifiers.TryGetValue(tile, out var modifier) && modifier.BlocksMovement) return false;
        return true;
    }

    public bool IsTileBlockingLOS(Vector2Int tile)
    {
        if (Room[tile] == RoomInfo.Tile.WALL) return true;
        if (TileModifiers.TryGetValue(tile, out var modifier) && modifier.BlocksLOS) return true;
        return false;
    }

    public bool IsLOSClear(Vector2Int from, Vector2Int to)
    {
        if (from == to) return true;
        return !Shapes.GridLine(from, to).Any(p => IsTileBlockingLOS(p));
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

    public float EvaluateCurrentState(Guid ActorGuid, AIProfile profile)
    {
        var self = CombatActors.ContainsKey(ActorGuid) ? CombatActors[ActorGuid] : null;
        var score = 0f;
        if (self == null) score -= profile.selfAliveWeight;
        var nearestEnemyDistance = CombatActors.Values.Where(actor => actor.Alignment != self.Alignment).Min(actor => (actor.Position - self.Position).sqrMagnitude);
        var nearestAllyDistance = CombatActors.Values.Where(actor => actor.Alignment == self.Alignment && actor != self).Min(actor => (actor.Position - self.Position).sqrMagnitude);
        score += Mathf.Sqrt(nearestEnemyDistance) * profile.distanceToNearestEnemyWeight;
        score += Mathf.Sqrt(nearestAllyDistance) * profile.distanceToNearestAllyWeight;

        foreach (var actor in CombatActors.Values)
        {
            var ownTeam = actor.Alignment == self.Alignment;
            var isSelf = actor == self;
            var healthWeight = isSelf ? profile.selfHealthWeight : ownTeam ? profile.alliesHealthTotalWeight : profile.enemiesHealthTotalWeight;
            score += ownTeam ? profile.alliesAliveCountWeight : profile.enemiesAliveCountWeight;
            score += healthWeight * actor.Health;
        }
        return score;
    }

    public CombatState(RoomInfo room, Dictionary<Guid, ICombatActor> combatActors, Dictionary<Vector2Int, ITileModifier> tileModifiers, Queue<Guid> turnQueue = null, Guid? activeActorGuid = null)
    {
        this.Room = room;
        this.CombatActors = combatActors;
        this.ActorPositions = new Dictionary<Vector2Int, Guid>(combatActors.Select(pair => new KeyValuePair<Vector2Int, Guid>(pair.Value.Position, pair.Key)));
        this.TileModifiers = tileModifiers;
        this.TurnQueue = turnQueue ?? new();
        this.ActiveActorGuid = activeActorGuid;
    }
}

public interface IReadOnlyCombatState
{
    public RoomInfo Room { get; }
    public IReadOnlyDictionary<Guid, IReadOnlyCombatActor> CombatActors { get; }
    public IReadOnlyDictionary<Vector2Int, Guid> ActorPositions { get; }
    public IReadOnlyDictionary<Vector2Int, ITileModifier> TileModifiers { get; }
    public IReadOnlyCollection<Guid> TurnQueue { get; }
    public Guid? ActiveActorGuid { get; }
    public IReadOnlyCombatActor ActiveActor => ActiveActorGuid.HasValue ? CombatActors[ActiveActorGuid.Value] : null;
    public bool IsTileWalkable(Vector2Int tile);
    public bool IsLOSClear(Vector2Int from, Vector2Int to);
    public CombatState DeepCopy();
    public float EvaluateCurrentState(IReadOnlyCombatActor actor, AIProfile profile) => EvaluateCurrentState(actor.Guid, profile);
    public float EvaluateCurrentState(Guid actorGuid, AIProfile profile);
}