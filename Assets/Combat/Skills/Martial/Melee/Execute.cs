using UnityEngine;

public class Execute : ISkill
{
    public string Description => "Strike at a target. If that kills it, gain a surge of action points.";
    public ClampedInt Cooldown { get; set; } = new(0, 1, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new ActorTargetSelector(true, 1)
    };
    public SkillGroup SkillGroup => SkillGroup.MELEE;
    public int Damage { get; set; } = 4;

    void ISkill.Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var position = (Vector2Int)parameters[0];
        if (!combatState.ActorPositions.ContainsKey(position)) return;
        var target = combatState.CombatActors[combatState.ActorPositions[position]];
        var result = combatState.DealDamage(user, target, DamageSources.PHYSICAL.WithDamageAmount(Damage));
        if (result.killed) user.ActionPoints += 4;
    }
}
