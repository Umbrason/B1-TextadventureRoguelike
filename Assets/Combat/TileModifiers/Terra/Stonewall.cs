using UnityEngine;

public class Stonewall : ITileModifier
{
    public Color Color => new Color(.3f, .2f, .1f);
    public char Char => '▓'; //░▒▓
    public int Duration { get; set; } = 3;
    public bool BlocksMovement => true;
    public bool BlocksLOS => true;
    public byte[] ByteData { get; set; } = new byte[0];
    public Element Element => Element.EARTH;
}
