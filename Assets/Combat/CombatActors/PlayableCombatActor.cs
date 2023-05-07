using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCombatActor : ICombatActor
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public int Alignment { get; set; }
    public Vector2Int Position { get; set; }
    private State state = new();
    public int AP { get => state.AP; set => state.AP = value; }
    IBinarySerializable ICombatActor.State => state;
    IReadOnlyList<ISkill> ISkillUser.Skills => state.skills;
    public int Initiative { get; set; }
    public ClampedInt Health { get; set ; }

    public static event Action<PlayableCombatActor> OnCharacterBeginTurn;

    private TurnInfo turnInfo;
    public void NotifyTurnStart(TurnInfo turnInfo)
    {
        this.turnInfo = turnInfo;
        OnCharacterBeginTurn.Invoke(this);
    }

    public void EndTurn()
    {
        turnInfo.EndTurn();
        turnInfo = null;
    }

    private class State : IBinarySerializable
    {
        public int AP;
        public readonly List<ISkill> skills = new();
        public byte[] Bytes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
