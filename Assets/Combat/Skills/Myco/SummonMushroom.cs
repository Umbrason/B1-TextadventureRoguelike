using UnityEngine;

public class SummonMushroom : ISkill, ISkillWithRange
{
    public string Description => "Summon a mushroom";
    public ClampedInt Cooldown { get; set; } = new(0, 0, 0);
    public int APCost { get; set; } = 1;
    public ITargetSelector[] TargetSelectors => new ITargetSelector[] {
        new RangedPositionSelector(true, true, Range)
    };

    public int Range { get; set; } = 10;
    public SkillGroup SkillGroup => SkillGroup.MYCOMANCY;

    public void Execute(CombatState combatState, ICombatActor user, params object[] parameters)
    {
        var pos = (Vector2Int)parameters[0];
        var mushroom = new Mushroom();
        mushroom.Position = pos;
        mushroom.Alignment = user.Alignment;
        combatState.AddActor(mushroom);
        if (combatState.Room.TileModifiers.TryGetValue(pos, out var modifier))
            combatState.ApplyStatus(mushroom, new ElementalSpores(modifier.Element));
    }
}
