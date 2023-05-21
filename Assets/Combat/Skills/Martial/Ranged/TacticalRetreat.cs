using UnityEngine;

public class TacticalRetreat : ISkill, ISkillWithRange
{
    public string Description => "Quickly jump to a more favourable position";
    public ClampedInt Cooldown { get; set; } = new(0, 2, 0);
    public int APCost { get; set; } = 2;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new RangedPositionSelector(false, true, Range)
    };
    public int Range { get; set; } = 15;
    public SkillGroup SkillGroup => SkillGroup.RANGED;
    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var position = (Vector2Int)parameters[0];
        combatState.TeleportActor(user, position);
    }
}
