using UnityEngine;

public class TipArrows : ISkill, ISkillWithRange
{
    public string Description => "Coat your arrows in a nearby surface to gain its element and 1.2x damage.";
    public ClampedInt Cooldown { get; set; } = new(0, 3, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new RangedPositionSelector(true, false, Range, (state, position) => state.Room.TileModifiers.ContainsKey(position)),
    };

    public int Range { get; set; } = 2;
    public SkillGroup SkillGroup => SkillGroup.RANGED;
    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var pos = (Vector2Int)parameters[0];
        var element = combatState.Room.TileModifiers[pos].Element;
        var status = new TippedArrows(element);
        combatState.ApplyStatus(user, status);
    }
}
