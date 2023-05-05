using UnityEngine;

public interface ICombatActor : IBinarySerializable
{
    public void NotifyTurnStart(TurnInfo turnInfo);
    public int Alignment { get; }
    public Vector2Int Position { get; set; }
}