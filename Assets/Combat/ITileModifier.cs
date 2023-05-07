using UnityEngine;

public interface ITileModifier : IBinarySerializable
{
    public Color Color { get; }
    public char Char { get; }
    public string Name { get; }
    public bool BlocksMovement { get; }
    public bool BlocksLOS { get; }
    public void OnEnter(ICombatActor actor) { }
    public void OnTurnBegin(ICombatActor actor) { }
    public void OnExit(ICombatActor actor) { }
}