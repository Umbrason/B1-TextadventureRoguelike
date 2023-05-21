using System.Linq;
using UnityEngine;

public class ArrowBarrage : ISkill, ISkillWithMultitarget, ISkillWithRange, ISkillWithDamage
{
    public string Description => "Fire multiple arrows.";
    public ClampedInt Cooldown { get; set; } = new(0, 3, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors =>
    Enumerable.Range(0, TargetCount).Select(
        _ => new ActorTargetSelector(true, Range)
    ).ToArray();

    public int Range { get; set; } = 30;
    public int TargetCount { get; set; } = 3;

    public SkillGroup SkillGroup => SkillGroup.RANGED;

    public int Damage { get; set; } = 3;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var positions = parameters.Select(p => (Vector2Int)p);
        foreach (var position in positions)
        {
            if (!combatState.ActorPositions.ContainsKey(position)) continue;
            var target = combatState.CombatActors[combatState.ActorPositions[position]];
            combatState.DealDamage(user, target, DamageSources.PHYSICAL.WithDamageAmount(Damage));
        }
    }
}
