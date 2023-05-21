using UnityEngine;

public class PiercingArrow : ISkill, ISkillWithRange, ISkillWithDamage
{
    public string Description => "Shoot a piercing arrow, that ignores targets armor";
    public ClampedInt Cooldown { get; set; } = new(0, 3, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new RangedPositionSelector(true, false, Range)
    };
    public int Range { get; set; } = 30;
    public int Damage { get; set; } = 5;
    public SkillGroup SkillGroup => SkillGroup.RANGED;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var position = (Vector2Int)parameters[0];
        if (!combatState.ActorPositions.ContainsKey(position)) return;
        var target = combatState.CombatActors[combatState.ActorPositions[position]];
        combatState.DealDamage(user, target, DamageSources.PHYSICAL.WithBypassArmor(true).WithDamageAmount(Damage));
    }
}
