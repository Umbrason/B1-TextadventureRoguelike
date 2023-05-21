using UnityEngine;

public class Grease : ITileModifier
{
    public Color Color => new Color(.6f, .8f, .4f);
    public Element Element => Element.PHYSICAL;
    public char Char => '▓';
    public bool BlocksMovement => false;
    public bool BlocksLOS => false;
    public byte[] ByteData { get; set; } = new byte[0];

    public void OnEnter(ICombatActor user, CombatState combatState) { 
        if((new System.Random().Next(0, 100) / 100f) < Balancing.GREASE_SLIP_CHANCE) user.StatusEffects.Add(new Prone());
    }
    public void OnExit(ICombatActor user, CombatState combatState) { }
    public void OnTurnBegin(ICombatActor user, CombatState combatState) { }
}
