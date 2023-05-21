using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class CombatState : IReadOnlyCombatState, IBinarySerializable
{
    public RoomInfo Room { get; set; }
    public Dictionary<Guid, ICombatActor> CombatActors { get; private set; }
    public Dictionary<Vector2Int, Guid> ActorPositions { get; private set; }
    public Queue<Guid> TurnQueue;
    public bool ActiveActorStunned => ActiveActor?.Stunned ?? false;
    public Guid? ActiveActorGuid { get; set; }
    public ICombatActor ActiveActor => ActiveActorGuid.HasValue && CombatActors.ContainsKey(ActiveActorGuid.Value) ? CombatActors[ActiveActorGuid.Value] : null;

    IReadOnlyDictionary<Guid, IReadOnlyCombatActor> IReadOnlyCombatState.CombatActors => CombatActors.Values.Select(actor => (IReadOnlyCombatActor)actor).ToDictionary(actor => actor.Guid);
    IReadOnlyCollection<Guid> IReadOnlyCombatState.TurnQueue => TurnQueue;
    IReadOnlyDictionary<Vector2Int, Guid> IReadOnlyCombatState.ActorPositions => ActorPositions;
    IReadOnlyRoomInfo IReadOnlyCombatState.Room => Room;

    public byte[] ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteIBinarySerializable(Room);
            stream.WriteDictionary(CombatActors, stream.WriteGuid, stream.WriteIBinarySerializable);
            stream.WriteDictionary(ActorPositions, stream.WriteVector2Int, stream.WriteGuid);
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
            TurnQueue = new(stream.ReadEnumerable(stream.ReadGuid));
            var guid = stream.ReadGuid();
            ActiveActorGuid = guid == Guid.Empty ? null : guid; ;
        }
    }

    public CombatState(RoomInfo room, Dictionary<Guid, ICombatActor> combatActors, Queue<Guid> turnQueue = null, Guid? activeActorGuid = null)
    {
        this.Room = room;
        this.CombatActors = combatActors;
        this.ActorPositions = new Dictionary<Vector2Int, Guid>(combatActors.Select(pair => new KeyValuePair<Vector2Int, Guid>(pair.Value.Position, pair.Key)));
        this.TurnQueue = turnQueue ?? new();
        this.ActiveActorGuid = activeActorGuid;
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
        if (Room.TileModifiers.TryGetValue(tile, out var modifier) && modifier.BlocksMovement) return false;
        return true;
    }

    public bool IsTileBlockingLOS(Vector2Int tile)
    {
        if (Room[tile] == RoomInfo.Tile.WALL) return true;
        if (Room.TileModifiers.TryGetValue(tile, out var modifier) && modifier.BlocksLOS) return true;
        return false;
    }

    public bool IsLOSClear(Vector2Int from, Vector2Int to)
    {
        if (from == to) return true;
        return !Shapes.GridLine(from, to).Any(p => IsTileBlockingLOS(p));
    }


    public CombatState SwapActors(IReadOnlyCombatActor actor1, IReadOnlyCombatActor actor2) => SwapActors(actor1.Guid, actor2.Guid);
    public CombatState SwapActors(Guid actorGuid1, Guid actorGuid2)
    {
        var actor1 = this.CombatActors[actorGuid1];
        var actor2 = this.CombatActors[actorGuid2];
        if (Room.TileModifiers.TryGetValue(actor1.Position, out var modifier)) modifier.OnExit(actor1, this);
        if (Room.TileModifiers.TryGetValue(actor2.Position, out modifier)) modifier.OnExit(actor2, this);
        ActorPositions.Remove(actor1.Position);
        ActorPositions.Remove(actor2.Position);
        var tmp = actor1.Position;
        actor1.Position = actor2.Position;
        actor2.Position = tmp;
        ActorPositions.Add(actor1.Position, actor1.Guid);
        ActorPositions.Add(actor2.Position, actor2.Guid);
        if (Room.TileModifiers.TryGetValue(actor1.Position, out modifier)) modifier.OnEnter(actor1, this);
        if (Room.TileModifiers.TryGetValue(actor2.Position, out modifier)) modifier.OnEnter(actor2, this);
        return this;
    }

    public CombatState TeleportActor(IReadOnlyCombatActor actor, Vector2Int targetLocation) => TeleportActor(actor.Guid, targetLocation);
    public CombatState TeleportActor(Guid actorGuid, Vector2Int targetLocation)
    {
        if (!IsTileWalkable(targetLocation)) return this;
        var actor = this.CombatActors[actorGuid];
        if (Room.TileModifiers.TryGetValue(actor.Position, out var modifier)) modifier.OnExit(actor, this);
        ActorPositions.Remove(actor.Position);
        actor.Position = targetLocation;
        ActorPositions.Add(actor.Position, actor.Guid);
        if (Room.TileModifiers.TryGetValue(actor.Position, out modifier)) modifier.OnEnter(actor, this);
        return this;
    }

    public CombatState AddActor(ICombatActor actor)
    {
        if (this.ActorPositions.ContainsKey(actor.Position)) return this;
        this.CombatActors.Add(actor.Guid, actor);
        this.ActorPositions.Add(actor.Position, actor.Guid);
        return this;
    }

    public CombatState DestroyActor(IReadOnlyCombatActor actor) => DestroyActor(actor.Guid);
    public CombatState DestroyActor(Guid actorGuid)
    {
        var actor = CombatActors[actorGuid];
        this.ActorPositions.Remove(CombatActors[actorGuid].Position);
        this.CombatActors.Remove(actorGuid);
        foreach (var status in actor.StatusEffects)
            status.OnDie(actor, this);
        return this;
    }

    public void RemoveTileModifier(Vector2Int position)
    {
        Room.TileModifiers.Remove(position);
    }
    public void SetTileModifier(Vector2Int position, ITileModifier tileModifier)
    {
        if (Room[position] != RoomInfo.Tile.FLOOR) return;
        Room.TileModifiers[position] = tileModifier;
        //propagate changes maybe?
    }

    public void ApplyStatus(IReadOnlyCombatActor actor, IStatusEffect statusEffect) => ApplyStatus(actor.Guid, statusEffect);
    public void ApplyStatus(Guid actorGuid, IStatusEffect statusEffect)
    {
        var rwActor = CombatActors[actorGuid];
        if (!statusEffect.IsStackable)
            rwActor.StatusEffects.RemoveAll(effect => effect.GetType() == statusEffect.GetType());
        rwActor.StatusEffects.Add(statusEffect);
        statusEffect.OnApply(rwActor, this);
    }

    public void RemoveStatus(IReadOnlyCombatActor actor, IStatusEffect statusEffect) => RemoveStatus(actor.Guid, statusEffect);
    public void RemoveStatus(Guid actorGuid, IStatusEffect statusEffect)
    {
        var rwActor = CombatActors[actorGuid];
        rwActor.StatusEffects.RemoveAll(effect => effect.GetType() == statusEffect.GetType());
        statusEffect.OnRemove(rwActor, this);
    }

    public DamageResultInfo DealDamage(IReadOnlyCombatActor source, IReadOnlyCombatActor target, DamageInfo attack) => ApplyDamage(source?.Guid, target.Guid, attack);
    public DamageResultInfo ApplyDamage(Guid? sourceGuid, Guid targetGuid, DamageInfo attack)
    {
        var rwSource = sourceGuid.HasValue ? CombatActors[sourceGuid.Value] : null;
        if (rwSource != null) foreach (var status in rwSource.StatusEffects)
                attack = status.OnBeforeDealDamage(rwSource, attack);
        var rwTarget = CombatActors[targetGuid];
        foreach (var status in rwTarget.StatusEffects)
            attack = status.OnBeforeRecieveDamage(rwTarget, attack);
        var damageResult = new DamageResultInfo();
        if (!attack.bypassArmor)
        {
            damageResult.armorDamage = Mathf.Min(rwTarget.Armor.Value, attack.amount);
            attack.amount -= damageResult.armorDamage;
            rwTarget.Armor -= damageResult.armorDamage;
        }
        damageResult.armorBroken = rwTarget.Armor == 0;
        damageResult.healthDamage = Mathf.Min(rwTarget.Health.Value, attack.amount);
        rwTarget.Health -= damageResult.healthDamage;
        damageResult.killed = rwTarget.Health.Value == 0;
        foreach (var status in rwTarget.StatusEffects)
            status.OnAfterRecieveDamage(rwTarget, damageResult);
        damageResult.killed = rwTarget.Health.Value == 0;
        ConsoleOutput.Println($"{rwTarget.Name} took {damageResult.armorDamage + damageResult.healthDamage}{(damageResult.killed ? " fatal" : "")} {attack.element} damage");
        if (rwSource != null) foreach (var status in rwSource.StatusEffects)
                status.OnAfterDealDamage(rwSource, damageResult);
        return damageResult;
    }

    public CombatState WithModifiedActor(ICombatActor actor)
    {
        var modifiedActors = new Dictionary<Guid, ICombatActor>(CombatActors);
        modifiedActors[actor.Guid] = actor;
        return this.WithActors(modifiedActors);
    }

    public CombatState WithRoom(RoomInfo room) => new(room, CombatActors, TurnQueue, ActiveActorGuid);
    public CombatState WithActors(Dictionary<Guid, ICombatActor> actors) => new(Room, actors, TurnQueue, ActiveActorGuid);
    public CombatState WithTurnQueue(Queue<Guid> turnQueue) => new(Room, CombatActors, turnQueue, ActiveActorGuid);
    public CombatState WithActiveActor(Guid? activeActorGuid) => new(Room, CombatActors, TurnQueue, activeActorGuid);

    private void BuildTurnQueue()
    {
        var teamDict = new SortedList<int, SortedList<int, ICombatActor>>();
        var CombatActorsWithInit = CombatActors.Values.Where(ca => ca.Initiative >= 0).ToArray();
        foreach (var actor in CombatActorsWithInit)
        {
            var bucket = teamDict.ContainsKey(actor.Alignment) ? teamDict[actor.Alignment] : teamDict[actor.Alignment] = new();
            bucket.Add(int.MaxValue - actor.Initiative, actor);
        }
        var alignments = teamDict.Keys.OrderBy(alignment => teamDict[alignment].Keys[0]).ToArray();
        int teamIndex = 0;
        while (TurnQueue.Count < CombatActorsWithInit.Length)
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
        if (self == null) return -profile.selfAliveWeight;
        var nearestEnemyDistance = float.MaxValue;
        var nearestAllyDistance = float.MaxValue;
        foreach (var actor in CombatActors.Values)
        {
            if (actor.Alignment < 0) continue;
            var ownTeam = actor.Alignment == self?.Alignment;
            var isSelf = actor == self;
            var pathfinder = new Pathfinder(p => IsTileWalkable(p) || p == actor.Position);
            var distance = pathfinder.FromTo(self.Position, actor.Position).Length;
            if (ownTeam && !isSelf && distance > 0) nearestAllyDistance = Mathf.Min(nearestAllyDistance, distance);
            if (!ownTeam && distance > 0) nearestEnemyDistance = Mathf.Min(nearestEnemyDistance, distance);
            var healthWeight = isSelf ? profile.selfHealthWeight : ownTeam ? profile.alliesHealthWeight : profile.enemiesHealthWeight;
            var armorWeight = isSelf ? profile.selfArmorWeight : ownTeam ? profile.alliesArmorWeight : profile.enemiesArmorWeight;
            score += ownTeam ? profile.alliesAliveCountWeight : profile.enemiesAliveCountWeight;
            score += healthWeight * actor.Health;
            score += armorWeight * actor.Armor;
        }
        score += nearestEnemyDistance * profile.distanceToNearestEnemyWeight;
        score += nearestAllyDistance * profile.distanceToNearestAllyWeight;
        return score;
    }
}

public interface IReadOnlyCombatState
{
    public IReadOnlyRoomInfo Room { get; }
    public IReadOnlyDictionary<Guid, IReadOnlyCombatActor> CombatActors { get; }
    public IReadOnlyDictionary<Vector2Int, Guid> ActorPositions { get; }
    public IReadOnlyCollection<Guid> TurnQueue { get; }
    public Guid? ActiveActorGuid { get; }
    public IReadOnlyCombatActor ActiveActor => ActiveActorGuid.HasValue ? CombatActors[ActiveActorGuid.Value] : null;
    public bool IsTileWalkable(Vector2Int tile);
    public bool IsLOSClear(Vector2Int from, Vector2Int to);
    public CombatState DeepCopy();
    public float EvaluateCurrentState(IReadOnlyCombatActor actor, AIProfile profile) => EvaluateCurrentState(actor.Guid, profile);
    public float EvaluateCurrentState(Guid actorGuid, AIProfile profile);
}