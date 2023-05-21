using UnityEngine;

public class ShootArrow : ISkill, ISkillWithRange, ISkillWithDamage
{
    public string Description => "Shoots an arrow at a single target in sight";
    public ClampedInt Cooldown { get; set; } = new(0, 0, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new ActorTargetSelector(true, Range)
    };
    public int Range { get; set; } = 30;
    public int Damage { get; set; } = 5;
    public SkillGroup SkillGroup => SkillGroup.RANGED;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var position = (Vector2Int)parameters[0];
        if (!combatState.ActorPositions.ContainsKey(position)) return;
        var target = combatState.CombatActors[combatState.ActorPositions[position]];
        combatState.DealDamage(user, target, DamageSources.PHYSICAL.WithDamageAmount(Damage));
    }
}
