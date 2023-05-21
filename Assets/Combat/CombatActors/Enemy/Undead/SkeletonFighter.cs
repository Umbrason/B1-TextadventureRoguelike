using System;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonFighter : ICombatActor, AICombatActor
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public Vector2Int Position { get; set; }
    public List<IStatusEffect> StatusEffects { get; set; } = new();

    public string Name { get; set; } = "Skeleton Fighter";
    public int Alignment { get; set; } = 1;
    public int Initiative { get; set; } = 2;
    public ClampedInt Armor { get; set; } = new(0, 6, 6);
    public ClampedInt Health { get; set; } = new(0, 10, 10);
    public ClampedInt ActionPoints { get; set; } = new(0, 3, 3);
    public ClampedInt MovementPoints { get; set; } = new(0, 5, 5);
    public int StunSources { get; set; }
    public List<ISkill> Skills { get; set; } = new() {
        new Slash() {Damage = 2}
    };
    public AIProfile Profile => AIProfiles.MELEE;

    public Color Color => new Color(.9f, .85f, .75f);
    public char Character => 'F';
}
