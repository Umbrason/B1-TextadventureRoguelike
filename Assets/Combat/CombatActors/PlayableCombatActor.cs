using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayableCombatActor : ICombatActor
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public Vector2Int Position { get; set; }
    public List<IStatusEffect> StatusEffects { get; set; } = new();

    public string Name { get; set; } = "Hero";
    public int Alignment { get; set; } = 0;
    public int Initiative { get; set; } = 10;
    public ClampedInt Armor { get; set; } = new(0, 5, 5);
    public ClampedInt Health { get; set; } = new(0, 30, 30);
    public ClampedInt ActionPoints { get; set; } = new(0, 7, 7);
    public ClampedInt MovementPoints { get; set; } = new(0, 10, 10);
    public int StunSources { get; set; }
    public abstract List<ISkill> Skills { get; set; }
    public abstract Color Color { get; }
    public char Character => Name[0];
}
