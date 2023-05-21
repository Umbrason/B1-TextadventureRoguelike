using System;
using System.Collections.Generic;
using UnityEngine;

public class ArchDevil : ICombatActor, AICombatActor
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public Vector2Int Position { get; set; }
    public List<IStatusEffect> StatusEffects { get; set; } = new();

    public string Name { get; set; } = "Arch Devil";
    public int Alignment { get; set; } = 1;
    public int Initiative { get; set; } = 1;
    public ClampedInt Armor { get; set; } = new(0, 5, 5);
    public ClampedInt Health { get; set; } = new(0, 25, 25);
    public ClampedInt ActionPoints { get; set; } = new(0, 6, 6);
    public ClampedInt MovementPoints { get; set; } = new(0, 8, 8);
    public List<ISkill> Skills { get; set; } = new() {
        new Slash(),
        new Fireball(),
        new Flamewall(),
        new ScorchingRay(),
        new ConvergeFlames(),
    };
    public AIProfile Profile => AIProfiles.RANGED;

    public int StunSources { get; set; }
    public Color Color => new Color(.8f, .2f, .0f);
    public char Character => 'A';
}

