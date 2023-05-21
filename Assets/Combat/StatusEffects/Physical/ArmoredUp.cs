public class ArmoredUp : IStatusEffect
{
    public int Duration { get; set; } = 2;

    const int ArmorAmount = 10;
    public void OnApply(ICombatActor actor, CombatState state)
    {
        actor.Armor.Max += ArmorAmount;
        actor.Armor.Value += ArmorAmount;
    }
    public void OnRemove(ICombatActor actor, CombatState state)
    {
        actor.Armor.Value -= ArmorAmount;
        actor.Armor.Max -= ArmorAmount;
    }
}

