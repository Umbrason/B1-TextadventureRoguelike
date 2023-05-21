using UnityEngine;

public class Caltrops : ITileModifier
{
    public Color Color => new Color(.7f, .7f, .7f);
    public Element Element => Element.PHYSICAL;
    public char Char => '▓';
    public bool BlocksMovement => false;
    public bool BlocksLOS => false;
    public byte[] ByteData { get; set; } = new byte[0];

    public void OnEnter(ICombatActor user, CombatState combatState)
    {
        combatState.DealDamage(null, user, DamageSources.BLEED);
    }
}
