using UnityEngine;

public interface ITileModifier : IBinarySerializable
{
    public Color Color { get; }
    public char Char { get; }
    public bool BlocksMovement { get; }
    public bool BlocksLOS { get; }
    public void OnEnter(ICombatActor user, CombatState combatState);
    public void OnTurnBegin(ICombatActor user, CombatState combatState);
    public void OnExit(ICombatActor user, CombatState combatState);
}