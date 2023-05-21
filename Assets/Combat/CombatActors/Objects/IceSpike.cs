using System;
using UnityEngine;
using System.Collections.Generic;

public class IceSpike : IObjectCombatActor
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public Vector2Int Position { get; set; }
    public string Name { get; set; } = "Ice Spike";
    public const int HEALTH = 5;
    public const int ARMOR = 0;
    public ClampedInt Armor { get; set; } = new(0, ARMOR, ARMOR);
    public ClampedInt Health { get; set; } = new(0, HEALTH, HEALTH);
    public int StunSources { get; set; }
    public List<IStatusEffect> StatusEffects { get; set; } = new();
    public int Alignment { get; set; }
    public Color Color => new Color(.7f, .9f, .9f);
    public char Character => 'I';
}
