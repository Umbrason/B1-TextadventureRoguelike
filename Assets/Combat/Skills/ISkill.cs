
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public interface ISkill : IReadOnlySkill, IBinarySerializable
{
    public string Description { get; }
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
            return stream.GetAllBytes();
        }
        set
        {
            var stream = new MemoryStream(value);
            Cooldown = stream.ReadClampedInt();
        }
    }
}

public interface IReadOnlySkill
{
    public IReadOnlyClampedInt Cooldown { get; }
    public int APCost { get; }
    public bool IsBuff { get; }
    public ITargetSelector[] TargetSelectors { get; }
    public bool CanUse(IReadOnlyCombatState combatState, IReadOnlyCombatActor user)
    {
        return Cooldown.Value == 0;
    }
}
