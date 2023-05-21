using UnityEngine;

public class Fire : ITileModifier
{
    public Color Color => new Color(1f, .3f, .2f);
    public Element Element => Element.FIRE;
    public char Char => '▓';
    public int Duration { get; set; } = 3;
    public bool BlocksMovement => false;
    public bool BlocksLOS => false;
    public byte[] ByteData { get; set; } = new byte[0];

    public void OnEnter(ICombatActor target, CombatState combatState)
    {
        combatState.DealDamage(null, target, DamageSources.FIRE);
    }
    public void OnTurnBegin(ICombatActor target, CombatState combatState)
    {
        combatState.DealDamage(null, target, DamageSources.FIRE);
    }
}
