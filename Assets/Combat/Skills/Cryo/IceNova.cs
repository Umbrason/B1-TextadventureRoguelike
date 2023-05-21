using System.Collections.Generic;
using System.Linq;

public class IceNova : ISkill, ISkillWithRadius, ISkillWithDamage
{
    public string Description => "Hit nearby enemies with a volley of frost.";
    public ClampedInt Cooldown { get; set; } = new(0, 3, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
    };
    public int Radius { get; set; } = 20;
    public int Damage { get; set; } = 4;

    public SkillGroup SkillGroup => SkillGroup.CRYOMANCY;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var enemiesInRange = combatState.ActorPositions
                            .Where(position => (position.Key - user.Position).sqrMagnitude < Radius * Radius)
                            .Select(pair => pair.Value)
                            .Select(combatState.CombatActors.GetValueOrDefault)
                            .Where(ActorTargetSelector => ActorTargetSelector.Alignment != user.Alignment);
        foreach (var enemy in enemiesInRange)
        {
            var result = combatState.DealDamage(user, enemy, DamageSources.ICE.WithDamageAmount(Damage));
            if (result.armorBroken) combatState.ApplyStatus(enemy, new Frozen());
        }
    }
}
