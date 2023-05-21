public class Enraged : IStatusEffect
{
    public int Duration { get; set; } = 1;

    private bool isMeleeSkill;
    public void OnUseSkill(ICombatActor actor, ISkill skill, CombatState state)
    {
        isMeleeSkill = skill.SkillGroup == SkillGroup.MELEE;
    }

    public DamageInfo OnBeforeDealDamage(ICombatActor actor, DamageInfo attackInfo) => attackInfo.WithDamageAmount(attackInfo.amount * 2);

}

