using UnityEngine;

public class Teleport : ISkill, ISkillWithRange
{
    public string Description => "Teleport a target to a new position";
    public ClampedInt Cooldown { get; set; } = new(0, 3, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new ActorTargetSelector(true, Range),
        new RangedPositionSelector(true, true, Range),
    };

    public int Range { get; set; } = 20;
    public SkillGroup SkillGroup => SkillGroup.AEROMANCY;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var actor = combatState.CombatActors[combatState.ActorPositions[(Vector2Int)parameters[0]]];
        combatState.TeleportActor(actor, (Vector2Int)parameters[1]);
    }
}
