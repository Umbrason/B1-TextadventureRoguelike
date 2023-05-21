using System.Collections.Generic;
using System.Linq;

public class Whirlwind : ISkill, ISkillWithRadius, ISkillWithDamage
{
    public string Description => "Hit nearby enemies with a swift spin of your blade.";
    public ClampedInt Cooldown { get; set; } = new(0, 3, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
    };
    public int Radius { get; set; } = 20;
    public int Damage { get; set; } = 5;

    public SkillGroup SkillGroup => SkillGroup.MELEE;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var enemiesInRange = combatState.ActorPositions
                            .Where(position => (position.Key - user.Position).sqrMagnitude < Radius * Radius)
                            .Select(pair => pair.Value)
                            .Select(combatState.CombatActors.GetValueOrDefault)
                            .Where(ActorTargetSelector => ActorTargetSelector.Alignment != user.Alignment);
        foreach (var enemy in enemiesInRange)
            combatState.DealDamage(user, enemy, DamageSources.PHYSICAL.WithDamageAmount(Damage));
    }
}
