using UnityEngine;

public class Web : ITileModifier
{
    public Color Color => new Color(1f, 1f, 1f);
    public char Char => '█';
    public bool BlocksMovement => false;
    public bool BlocksLOS => false;
    public byte[] ByteData { get; set; } = new byte[0];

    public void OnEnter(ICombatActor user, CombatState combatState) {
        if(Random.value < Balancing.WEB_ENWEB_CHANCE) user.StatusEffects.Add(new Enwebbed());
     }
    public void OnExit(ICombatActor user, CombatState combatState) { }
    public void OnTurnBegin(ICombatActor user, CombatState combatState) { }
}
