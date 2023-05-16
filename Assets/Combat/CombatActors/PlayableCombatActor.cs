using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCombatActor : ICombatActor
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public Vector2Int Position { get; set; }
    public List<IStatusEffect> StatusEffects { get; set; } = new();

    public string Name { get; set; } = "Hero";
    public int Alignment { get; set; } = 0;
    public int Initiative { get; set; } = 10;
    public ClampedInt Armor { get; set; } = new(0, 0, 0);
    public ClampedInt Health { get; set; } = new(0, 10, 10);
    public ClampedInt ActionPoints { get; set; } = new(0, 5, 5);
    public ClampedInt MovementPoints { get; set; } = new(0, 5, 5);
    public List<ISkill> Skills { get; set; } = new() { new Fireball() };
}
