using System;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : IObjectCombatActor
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public Vector2Int Position { get; set; }
    public string Name { get; set; } = "Barricade";
    public const int HEALTH = 3;
    public ClampedInt Armor { get; set; } = new(0, 5, 5);
    public ClampedInt Health { get; set; } = new(0, HEALTH, HEALTH);
    public int StunSources { get; set; }
    public List<IStatusEffect> StatusEffects { get; set; } = new();
    public int Alignment { get; set; }
    public Color Color => new Color(.6f, .3f, .3f);
    public char Character => '#';
}