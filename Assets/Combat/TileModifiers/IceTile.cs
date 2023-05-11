
using UnityEngine;

public class IceTile : ITileModifier
{
    public Color Color => new Color(.2f, .5f, 1f);
    public char Char => '#';    
    public bool BlocksMovement => false;
    public bool BlocksLOS => false;
    public byte[] ByteData { get; set; } = new byte[0];
    
    public void OnEnter(ICombatActor actor, CombatLog log)
    {

    }
}
