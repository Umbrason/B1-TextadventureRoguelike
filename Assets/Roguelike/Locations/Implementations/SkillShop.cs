using System.IO;
using System.Linq;

public class SkillShop : IWorldLocation
{
    public SkillShop() { }
    public SkillShop(SkillGroup group)
    {
        skillGroup = group;
        skills = RandomSkills(options, group);
        prices = RandomPrices(options);
    }
    public byte[] ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteEnumerable(skills, stream.WriteIBinarySerializable);
            stream.WriteEnumerable(prices, stream.WriteInt);
            stream.WriteEnum(skillGroup);
            return stream.GetAllBytes();
        }
        set
        {
            var stream = new MemoryStream(value);
            skills = stream.ReadEnumerable(stream.ReadIBinarySerializable<ISkill>);
            prices = stream.ReadEnumerable(stream.ReadInt);
            skillGroup = stream.ReadEnum<SkillGroup>();
        }
    }
    public string Name => "SkillShop";
    public string storyText => $"You've stumbled upon a travelling merchant and take a look at what they have to offer.\nThey seem to sell a variety of books about {skillGroup} skills.\n You have {RunManager.ReadOnlyRunInfo.Gold} Gold ";

    const int options = 3;
    private SkillGroup skillGroup;
    public SkillGroup SkillGroup => skillGroup;
    private ISkill[] skills;
    private int[] prices;
    public string[] optionTexts => Enumerable.Range(0, skills.Length).Select(i => $"{(RunManager.ReadOnlyRunInfo.ReadOnlyPartyInfo.PartyLeader.Skills.Any(skill => skill.GetType() == skills[i].GetType()) || prices[i] > RunManager.ReadOnlyRunInfo.Gold ? "<s>" : "")}{prices[i]} gp: {skills[i].GetType().Name}{(RunManager.ReadOnlyRunInfo.ReadOnlyPartyInfo.PartyLeader.Skills.Any(skill => skill.GetType() == skills[i].GetType()) || prices[i] > RunManager.ReadOnlyRunInfo.Gold ? "</s>" : "")}").Append("leave").ToArray();

    public static ISkill[] RandomSkills(int count, SkillGroup skillGroup) => IBinarySerializableFactory<ISkill>.Types.Select(skill => skill.Key).Select(IBinarySerializableFactory<ISkill>.CreateDefault).Where(skill => skill.SkillGroup == skillGroup).ToArray().RandomElements(options).ToArray();
    public static int[] RandomPrices(int count) => Enumerable.Range(0, count).Select(_ => UnityEngine.Random.Range(15, 45)).ToArray();
    public void OnPickOption(int option, RunInfo runInfo)
    {
        if (option >= skills.Length) { RunManager.ShowWorldMap(); return; } //exit
        if (RunManager.ReadOnlyRunInfo.ReadOnlyPartyInfo.PartyLeader.Skills.Any(skill => skill.GetType() == skills[option].GetType())) return;
        if (runInfo.Gold > prices[option])
        {
            runInfo.Gold -= prices[option];
            runInfo.party.PartyLeader.Skills.Add(skills[option]);
        }
    }
}