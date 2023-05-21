using System.IO;

public interface ISkill : IReadOnlySkill, IBinarySerializable
{
    public new ClampedInt Cooldown { get; set; }
    public new int APCost { get; set; }
    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters);
    bool IReadOnlySkill.IsBuff => false;
    IReadOnlyClampedInt IReadOnlySkill.Cooldown => Cooldown;
    int IReadOnlySkill.APCost => APCost;
    byte[] IBinarySerializable.ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteClampedInt(Cooldown);
            if (this is ISkillWithRadius) stream.WriteInt(((ISkillWithRadius)this).Radius);
            if (this is ISkillWithRange) stream.WriteInt(((ISkillWithRange)this).Range);
            if (this is ISkillWithMultitarget) stream.WriteInt(((ISkillWithMultitarget)this).TargetCount);
            return stream.GetAllBytes();
        }
        set
        {
            var stream = new MemoryStream(value);
            Cooldown = stream.ReadClampedInt();
            if (this is ISkillWithRadius) ((ISkillWithRadius)this).Radius = stream.ReadInt();
            if (this is ISkillWithRange) ((ISkillWithRange)this).Range = stream.ReadInt();
            if (this is ISkillWithMultitarget) ((ISkillWithMultitarget)this).TargetCount = stream.ReadInt();
        }
    }
}

public interface IReadOnlySkill
{
    public string Description { get; }
    public SkillGroup SkillGroup { get; }
    public IReadOnlyClampedInt Cooldown { get; }
    public int APCost { get; }
    public bool IsBuff { get; }
    public ITargetSelector[] TargetSelectors { get; }
    public bool CanUse(IReadOnlyCombatState combatState, IReadOnlyCombatActor user)
    {
        return Cooldown.Value == 0 && user.ActionPoints.Value >= APCost;
    }
}
