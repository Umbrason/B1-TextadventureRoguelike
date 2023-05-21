using System;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMage : ICombatActor, AICombatActor
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public Vector2Int Position { get; set; }
    public List<IStatusEffect> StatusEffects { get; set; } = new();

    public string Name { get; set; } = "Skeleton Mage";
    public int Alignment { get; set; } = 1;
    public int Initiative { get; set; } = 4;
    public ClampedInt Armor { get; set; } = new(0, 2, 2);
    public ClampedInt Health { get; set; } = new(0, 6, 6);
    public ClampedInt ActionPoints { get; set; } = new(0, 3, 3);
    public ClampedInt MovementPoints { get; set; } = new(0, 3, 3);
    public int StunSources { get; set; }
    public List<ISkill> Skills { get; set; } = new() {
        new Fireball(),
        new NecroticAura(),
        new LifeDrain(),
        new NecroticTouch(),
    };
    public AIProfile Profile => AIProfiles.RANGED;

    public Color Color => new Color(.9f, .85f, .75f);
    public char Character => 'M';
}
