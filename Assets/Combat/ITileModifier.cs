using UnityEngine;

public interface ITileModifier : IBinarySerializable
{
    public Color Color { get; }
    public char Char { get; }
    public bool BlocksMovement { get; }
    public bool BlocksLOS { get; }
    public virtual void OnEnter(ICombatActor actor, CombatLog log) { }
    public virtual void OnTurnBegin(ICombatActor actor, CombatLog log) { }
    public virtual void OnExit(ICombatActor actor, CombatLog log) { }
}