using UnityEngine;

public class Slash : ISkill, ISkillWithDamage
{
    public string Description => "Strike at a single target in melee range";
    public ClampedInt Cooldown { get; set; } = new(0, 0, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new ActorTargetSelector(true, 1)
    };
    public SkillGroup SkillGroup => SkillGroup.MELEE;

    public int Damage { get; set; } = 4;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var position = (Vector2Int)parameters[0];
        if (!combatState.ActorPositions.ContainsKey(position)) return;
        var target = combatState.CombatActors[combatState.ActorPositions[position]];
        combatState.DealDamage(user, target, DamageSources.PHYSICAL.WithDamageAmount(Damage));
    }
}
