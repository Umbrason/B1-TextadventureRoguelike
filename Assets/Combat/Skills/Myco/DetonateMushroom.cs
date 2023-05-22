using System.Linq;
using UnityEngine;

public class DetonateMushroom : ISkill, ISkillWithRange, ISkillWithDamage
{
    public string Description => "Detonate a mushroom for massive damage!";
    public ClampedInt Cooldown { get; set; } = new(0, 4, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new ActorTargetSelector(true, Range, false, (state, p) => state.CombatActors[state.ActorPositions[p]] is Mushroom)
    };

    public int Range { get; set; } = 20;
    public int Radius { get; set; } = 8;
    public int Damage { get; set; } = 12;

    public SkillGroup SkillGroup => SkillGroup.MYCOMANCY;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var mushroom = combatState.CombatActors[combatState.ActorPositions[(Vector2Int)parameters[0]]];
        var targetPosition = mushroom.Position;
        var area = Shapes.GridCircle(targetPosition, Radius);
        var actorsInArea = area.Where(p => combatState.ActorPositions.ContainsKey(p))
                            .Select(p => combatState.CombatActors[combatState.ActorPositions[p]])
                            .Where(a => a.Alignment != user.Alignment)
                            .ToArray();
        var element = (mushroom.StatusEffects.FirstOrDefault(s => s is ElementalSpores) as ElementalSpores)?.element ?? Element.FUNGAL;
        foreach (var target in actorsInArea) combatState.DealDamage(user, target, DamageSources.FUNGAL.WithElement(element).WithDamageAmount(Damage));

    }
}
