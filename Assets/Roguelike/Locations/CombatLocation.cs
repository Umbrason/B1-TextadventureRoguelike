using System.IO;
using UnityEngine;

public class CombatLocation : IWorldLocation
{
    public CombatLocation(LocationData locationData, CombatEncounterInfo encounterInfo)
    {
        
        this.Name = locationData.name;
        this.Image = locationData.Sprite;
        this.storyText = locationData.StoryText;
        this.optionTexts = new string[] { "Fight!" };
        this.EncounterInfo = encounterInfo;
    }

    public string Name { get; private set; }
    public CombatEncounterInfo EncounterInfo { get; private set; }
    byte[] IBinarySerializable.ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteIBinarySerializable(EncounterInfo);
            stream.WriteString(Name);
            return stream.GetAllBytes();
        }
        set
        {
            var stream = new MemoryStream(value);
            EncounterInfo = stream.ReadIBinarySerializable<CombatEncounterInfo>();
            Name = stream.ReadString();
        }
    }
    public Sprite Image { get; private set; }
    public string storyText { get; private set; } = "";
    public string[] optionTexts { get; private set; } = new string[0];
    public void OnPickOption(int option)
    {
        RunManager.StartCombat(EncounterInfo);
    }
}