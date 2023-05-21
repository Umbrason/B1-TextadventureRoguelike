using UnityEngine;

[CreateAssetMenu(menuName = "CombatEncounterInfoData")]
public class CombatEncounterInfoData : ScriptableObject
{
    public Vector2Int[] AllyStartPositions;
    public Vector2Int[] EnemyStartPositions;
    public string[] EnemyTypes;
    public TextAsset roomLayoutFile;    
}