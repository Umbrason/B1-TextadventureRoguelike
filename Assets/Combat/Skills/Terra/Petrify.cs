using UnityEngine;

public class Petrify : ISkill, ISkillWithRange
{
    public string Description => "Petrify target character, stunning them but also slightly increasing their resistance.";
    public ClampedInt Cooldown { get; set; } = new(0, 3, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new ActorTargetSelector(true, Range)
    };

    public SkillGroup SkillGroup => SkillGroup.TERRAMANCY;
    public int Range { get; set; } = 5;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var position = (Vector2Int)parameters[0];
        if (!combatState.ActorPositions.ContainsKey(position)) return;
        var target = combatState.CombatActors[combatState.ActorPositions[position]];
        combatState.ApplyStatus(target, new Petrified());
    }
}
