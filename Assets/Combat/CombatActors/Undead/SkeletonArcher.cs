using System;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcher : ICombatActor
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public Vector2Int Position { get; set; }
    public List<IStatusEffect> StatusEffects { get; set; } = new();

    public string Name { get; set; } = "SkeletonArcher";
    public int Alignment { get; set; }
    public int Initiative { get; set; }
    public ClampedInt Armor { get; set; }
    public ClampedInt Health { get; set; }
    public ClampedInt ActionPoints { get; set; }
    public ClampedInt MovementPoints { get; set; }
    public List<ISkill> Skills { get; set; } = new();
}