using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Mushroom : IObjectCombatActor
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public Vector2Int Position { get; set; }
    public string Name { get; set; } = "Mushroom";
    public const int HEALTH = 2;
    public ClampedInt Armor { get; set; } = new(0, 0, 0);
    public ClampedInt Health { get; set; } = new(0, HEALTH, HEALTH);
    public int StunSources { get; set; }
    public List<IStatusEffect> StatusEffects { get; set; } = new();
    public int Alignment { get; set; }
    public Color Color => new Color(.3f, .8f, .3f);
    public char Character => 'o';
}