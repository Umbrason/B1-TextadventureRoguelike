using UnityEngine;

public class Ignite : ISkill, ISkillWithRange, ISkillWithDamage
{
    public string Description => "Ignite a flamable surface or creature";
    public ClampedInt Cooldown { get; set; } = new(0, 0, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new ActorTargetSelector(requiresLOS: true, range: Range)
    };
    public int Range { get; set; } = 10;
    public SkillGroup SkillGroup => SkillGroup.PYROMANCY;

    public int Damage { get; set; } = 3;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var targetPosition = (Vector2Int)parameters[0];
        if (!combatState.ActorPositions.TryGetValue(targetPosition, out var guid)) return;
        var target = combatState.CombatActors[combatState.ActorPositions[targetPosition]];
        var result = combatState.DealDamage(user, target, DamageSources.FIRE.WithDamageAmount(Damage));
        if (result.armorBroken) combatState.ApplyStatus(target, new Burning());
    }
}
