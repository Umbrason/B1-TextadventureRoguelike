using System.Linq;
using UnityEngine;

public abstract class CombatLocation : IWorldLocation
{
    public abstract string Name { get; }
    public virtual string LocationImage { get => Name; }
    private CombatEncounterInfoData cached_data;
    private CombatEncounterInfoData data => cached_data ??= Resources.Load<CombatEncounterInfoData>($"CombatEncounters/{Name}");
    private CombatEncounterInfo cached_EncounterInfo;
    public CombatEncounterInfo EncounterInfo => cached_EncounterInfo ??= GenerateEncounterInfo();
    private CombatEncounterInfo GenerateEncounterInfo()
    {
        var enemies = data.EnemyTypes.Select(enemy => IBinarySerializableFactory<ICombatActor>.CreateDefault(enemy)).ToArray();
        for (int i = 0; i < enemies.Length; i++) enemies[i].Position = data.EnemyStartPositions[i];
        return new CombatEncounterInfo(new RoomInfo(data.roomLayoutFile.text), enemies, data.AllyStartPositions);
    }

    byte[] IBinarySerializable.ByteData
    {
        get => new byte[0];
        set { }
    }
    public abstract string storyText { get; }
    public string[] optionTexts => new string[] { "Fight" };
    public void OnPickOption(int option, RunInfo run) => RunManager.StartCombat(EncounterInfo);
}