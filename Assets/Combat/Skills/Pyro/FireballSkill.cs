
public class FireballSkill : ISkill
{
    public int Cooldown { get; set; }
    public ISkill.ITargetSelector[] TargetSelectors => new ISkill.ITargetSelector[0];
    public byte[] ByteData { get; set; }
    public bool CanUse(CombatState combatState, ICombatActor user)
    {
        return Cooldown == 0;
    }
    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        
    }
}
