using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;

public class PowerWell : IWorldLocation
{


    public byte[] ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteEnumerable(skillIndices, stream.WriteInt);
            return stream.GetAllBytes();
        }
        set
        {
            var stream = new MemoryStream(value);
            skillIndices = stream.ReadEnumerable(stream.ReadInt);
        }
    }

    //decrease AP
    //decrease CD
    //increase range
    //increase radius
    //increase target amount
    //increase flat damage

    private int[] skillIndices = Enumerable.Range(0, 6).Select(_ => Random.Range(0, 1000)).ToArray(); //6

    public string Name => "PowerWell";
    public string storyText => "You can feel an abundance of power crackle in the air.\nLet the power flow into one of your abilities?";
    public string[] optionTexts => getOptionTexts();

    private string[] getOptionTexts()
    {
        var skills = RunManager.ReadOnlyRunInfo.ReadOnlyPartyInfo.PartyLeader.Skills;

        var APSkills = skills.Where(skill => skill.APCost > 0).ToArray();
        var CDSkills = skills.Where(skill => skill.Cooldown.Max > 0).ToArray();
        var rangeSkills = skills.Where(skill => skill is ISkillWithRange).ToArray();
        var radiusSkills = skills.Where(skill => skill is ISkillWithRadius).ToArray();
        var multiTargetSkills = skills.Where(skill => skill is ISkillWithMultitarget).ToArray();
        var damageSkills = skills.Where(skill => skill is ISkillWithDamage).ToArray();

        var APSkill = APSkills.Length > 0 ? APSkills[skillIndices[0] % APSkills.Length] : null;
        var CDSkill = CDSkills.Length > 0 ? CDSkills[skillIndices[1] % CDSkills.Length] : null;
        var rangeSkill = rangeSkills.Length > 0 ? rangeSkills[skillIndices[2] % rangeSkills.Length] : null;
        var radiusSkill = radiusSkills.Length > 0 ? radiusSkills[skillIndices[3] % radiusSkills.Length] : null;
        var multiTargetSkill = multiTargetSkills.Length > 0 ? multiTargetSkills[skillIndices[4] % multiTargetSkills.Length] : null;
        var damageSkill = damageSkills.Length > 0 ? damageSkills[skillIndices[5] % damageSkills.Length] : null;

        var options = new List<string>();
        if (APSkill != null) options.Add($"{APSkill.GetType().Name} -1 APCost");
        if (CDSkill != null) options.Add($"{CDSkill.GetType().Name} -1 Cooldown");
        if (rangeSkill != null) options.Add($"{rangeSkill.GetType().Name} +4 Range");
        if (radiusSkill != null) options.Add($"{radiusSkill.GetType().Name} +2 Radius");
        if (multiTargetSkill != null) options.Add($"{multiTargetSkill.GetType().Name} +1 Target");
        if (damageSkill != null) options.Add($"{damageSkill.GetType().Name} +2 Damage");
        options.Add("leave");
        return options.ToArray();
    }

    public void OnPickOption(int option, RunInfo runInfo)
    {
        var skills = runInfo.party.PartyLeader.Skills;
        var APSkills = skills.Where(skill => skill.APCost > 0).ToArray();
        var CDSkills = skills.Where(skill => skill.Cooldown.Max > 0).ToArray();
        var rangeSkills = skills.Where(skill => skill is ISkillWithRange).ToArray();
        var radiusSkills = skills.Where(skill => skill is ISkillWithRadius).ToArray();
        var multiTargetSkills = skills.Where(skill => skill is ISkillWithMultitarget).ToArray();
        var damageSkills = skills.Where(skill => skill is ISkillWithDamage).ToArray();
        var APSkill = APSkills.Length > 0 ? APSkills[skillIndices[0] % APSkills.Length] : null;
        var CDSkill = CDSkills.Length > 0 ? CDSkills[skillIndices[1] % CDSkills.Length] : null;
        var rangeSkill = rangeSkills.Length > 0 ? (ISkillWithRange)rangeSkills[skillIndices[2] % rangeSkills.Length] : null;
        var radiusSkill = radiusSkills.Length > 0 ? (ISkillWithRadius)radiusSkills[skillIndices[3] % radiusSkills.Length] : null;
        var multiTargetSkill = multiTargetSkills.Length > 0 ? (ISkillWithMultitarget)multiTargetSkills[skillIndices[4] % multiTargetSkills.Length] : null;
        var damageSkill = damageSkills.Length > 0 ? (ISkillWithDamage)damageSkills[skillIndices[5] % damageSkills.Length] : null;
        if (APSkill == null && option >= 0) option++;
        if (CDSkill == null && option >= 1) option++;
        if (rangeSkill == null && option >= 2) option++;
        if (radiusSkill == null && option >= 3) option++;
        if (multiTargetSkill == null && option >= 4) option++;
        if (damageSkill == null && option >= 5) option++;
        switch (option)
        {
            case 0:
                APSkill.APCost -= 1;
                break;
            case 1:
                CDSkill.Cooldown.Max -= 1;
                break;
            case 2:
                rangeSkill.Range += 4;
                break;
            case 3:
                radiusSkill.Radius += 2;
                break;
            case 4:
                multiTargetSkill.TargetCount += 1;
                break;
            case 5:
                damageSkill.Damage += 2;
                break;
        }
        RunManager.ShowWorldMap();
    }
}