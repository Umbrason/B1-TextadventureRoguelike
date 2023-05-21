using UnityEngine;

public class Supercool : ISkill, ISkillWithRange
{
    public string Description => "Instantly freeze a target character without armor";
    public ClampedInt Cooldown { get; set; } = new(0, 4, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new ActorTargetSelector(true, Range)
    };

    public int Range { get; set; } = 20;
    public SkillGroup SkillGroup => SkillGroup.CRYOMANCY;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var pos = (Vector2Int)parameters[0];
        if(!combatState.ActorPositions.TryGetValue(pos, out var guid)) return;
        var target = combatState.CombatActors[guid];
        if(target.Armor.Value > 0) return;
        combatState.ApplyStatus(target, new Frozen());
    }
}
