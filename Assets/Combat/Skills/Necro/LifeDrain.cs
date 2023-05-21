using UnityEngine;

public class LifeDrain : ISkill, ISkillWithDamage, ISkillWithRange
{
    public string Description => "Siphon from your enemies health";
    public ClampedInt Cooldown { get; set; } = new(0, 1, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new ActorTargetSelector(true, Range)
    };
    public SkillGroup SkillGroup => SkillGroup.NECROMANCY;
    public int Damage { get; set; } = 4;
    public int Range { get; set; } = 2;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var position = (Vector2Int)parameters[0];
        if (!combatState.ActorPositions.ContainsKey(position)) return;
        var target = combatState.CombatActors[combatState.ActorPositions[position]];
        var result = combatState.DealDamage(user, target, DamageSources.NECROTIC.WithBypassArmor(true).WithDamageAmount(Damage));        
        user.Health += result.healthDamage;
    }
}
