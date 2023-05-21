using System.Linq;
using UnityEngine;

public class ShatterIce : ISkill, ISkillWithRadius, ISkillWithDamage, ISkillWithRange
{
    public string Description => "Cause all nearby ice to shatter, dealing damage to enemies next to it.";
    public ClampedInt Cooldown { get; set; } = new(0, 1, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
    };

    public SkillGroup SkillGroup => SkillGroup.CRYOMANCY;

    public int Range { get; set; } = 20;
    public int Radius { get; set; } = 2;
    public int Damage { get; set; } = 3;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var center = user.Position;
        var shape = Shapes.GridCircle(center, Range);
        foreach (var pos in shape)
        {
            var hasIce = combatState.Room.TileModifiers.TryGetValue(pos, out var tileModifier) && tileModifier.GetType() == typeof(Ice);
            var spike = combatState.ActorPositions.TryGetValue(pos, out var actorGuid) ? combatState.CombatActors[actorGuid] as IceSpike : null;
            var hasSpike = spike != null;
            if (hasSpike) combatState.DestroyActor(spike);
            if (hasIce) combatState.RemoveTileModifier(pos);
            var damage = (hasSpike ? Damage : 0) + (hasIce ? Damage : 0);

            var area = Shapes.GridCircle(pos, Radius);
            var actorsInArea = area.Where(p => combatState.ActorPositions.ContainsKey(p)).Select(p => combatState.CombatActors[combatState.ActorPositions[p]]).Where(actor => actor.Alignment != user.Alignment).ToArray();
            foreach (var enemy in actorsInArea)
                combatState.DealDamage(user, enemy, DamageSources.PHYSICAL.WithDamageAmount(Damage));
        }
    }
}
