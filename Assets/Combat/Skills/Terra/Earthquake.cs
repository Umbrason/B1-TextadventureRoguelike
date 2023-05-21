using System.Collections.Generic;
using System.Linq;

public class Earthquake : ISkill, ISkillWithRadius, ISkillWithDamage
{
    public string Description => "Shake the ground causing nearby enemies to fall over.";
    public ClampedInt Cooldown { get; set; } = new(0, 5, 0);
    public int APCost { get; set; } = 3;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
    };

    public int Radius { get; set; } = 20;
    public int Damage { get; set; } = 3;
    public SkillGroup SkillGroup => SkillGroup.TERRAMANCY;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var enemiesInRange = combatState.ActorPositions
                    .Where(position => (position.Key - user.Position).sqrMagnitude < Radius * Radius)
                    .Select(pair => pair.Value)
                    .Select(combatState.CombatActors.GetValueOrDefault)
                    .Where(ActorTargetSelector => ActorTargetSelector.Alignment != user.Alignment);
        foreach (var enemy in enemiesInRange)
        {
            var result = combatState.DealDamage(user, enemy, DamageSources.EARTH.WithDamageAmount(Damage));
            combatState.ApplyStatus(enemy, new Prone());
        }
    }
}
