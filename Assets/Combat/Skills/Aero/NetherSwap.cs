using UnityEngine;

public class NetherSwap : ISkill, ISkillWithRange
{
    public string Description => "Swap two targets positions.";
    public ClampedInt Cooldown { get; set; } = new(0, 2, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new ActorTargetSelector(true, Range),
        new ActorTargetSelector(true, Range),
    };

    public int Range { get; set; } = 20;
    public SkillGroup SkillGroup => SkillGroup.AEROMANCY;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var actor1 = combatState.ActorPositions[(Vector2Int)parameters[0]];
        var actor2 = combatState.ActorPositions[(Vector2Int)parameters[1]];
        combatState.SwapActors(actor1, actor2);
    }
}
