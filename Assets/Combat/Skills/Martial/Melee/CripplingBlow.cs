using UnityEngine;

public class CripplingBlow : ISkill
{
    public string Description => "Strike your enemy with a mighty blow that leaves them unable to move.";

    public ClampedInt Cooldown { get; set; } = new(0, 3, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new ActorTargetSelector(true, 1)
    };
    public SkillGroup SkillGroup => SkillGroup.MELEE;

    public int Damage { get; set; } = 7;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var position = (Vector2Int)parameters[0];
        if (!combatState.ActorPositions.ContainsKey(position)) return;
        var target = combatState.CombatActors[combatState.ActorPositions[position]];
        var result = combatState.DealDamage(user, target, DamageSources.PHYSICAL.WithDamageAmount(Damage));
        if (result.armorBroken) combatState.ApplyStatus(target, new Crippled());
    }
}
