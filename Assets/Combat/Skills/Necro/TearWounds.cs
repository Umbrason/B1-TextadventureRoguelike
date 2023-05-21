using System.Linq;
using UnityEngine;

public class TearWounds : ISkill, ISkillWithRange
{
    public string Description => "Deepen an enemies wounds, doubling their bleed effects";
    public ClampedInt Cooldown { get; set; } = new(0, 3, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new ActorTargetSelector(true, Range)
    };

    public int Range { get; set; } = 15;
    public SkillGroup SkillGroup => SkillGroup.NECROMANCY;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var target = combatState.CombatActors[combatState.ActorPositions[(Vector2Int)parameters[0]]];
        var bleedCount = target.StatusEffects.Where(effect => effect is Bleeding).Count();
        for (int i = 0; i < bleedCount; i++)
        { 
            combatState.ApplyStatus(user, new Bleeding());
        }
    }
}
