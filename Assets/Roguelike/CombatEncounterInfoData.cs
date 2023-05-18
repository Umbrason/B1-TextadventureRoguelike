using UnityEngine;

[CreateAssetMenu(menuName = "CombatEncounterInfoData")]
public class CombatEncounterInfoData : ScriptableObject
{
    public Vector2Int[] AllyStartPositions;
    public TextAsset roomLayoutFile;    
}