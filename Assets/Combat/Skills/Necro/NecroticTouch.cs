using UnityEngine;

public class NecroticTouch : ISkill, ISkillWithDamage
{
    public string Description => "Touch a nearby enemy to inflict bleeding and massive damage";
    public ClampedInt Cooldown { get; set; } = new(0, 3, 0);
    public int APCost { get; set; } = 3;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new ActorTargetSelector(true, 1)
    };
    public SkillGroup SkillGroup => SkillGroup.NECROMANCY;
    public int Damage { get; set; } = 10;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var position = (Vector2Int)parameters[0];
        if (!combatState.ActorPositions.ContainsKey(position)) return;
        var target = combatState.CombatActors[combatState.ActorPositions[position]];
        combatState.DealDamage(user, target, DamageSources.NECROTIC.WithDamageAmount(Damage));
        combatState.ApplyStatus(target, new Bleeding());
    }
}
