using UnityEngine;

public class Smoke : ITileModifier
{
    public Color Color => new Color(.55f, .5f, .45f);
    public Element Element => Element.FIRE;
    public char Char => '░';
    public int Duration { get; set; } = 3;
    public bool BlocksMovement => false;
    public bool BlocksLOS => true;
    public byte[] ByteData { get; set; } = new byte[0];

    public void OnEnter(ICombatActor user, CombatState combatState) { }
    public void OnExit(ICombatActor user, CombatState combatState) { }
    public void OnTurnBegin(ICombatActor user, CombatState combatState) { }
}
