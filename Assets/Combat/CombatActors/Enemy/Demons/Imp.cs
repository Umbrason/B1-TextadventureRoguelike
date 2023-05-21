using System;
using System.Collections.Generic;
using UnityEngine;

public class Imp : ICombatActor, AICombatActor
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public Vector2Int Position { get; set; }
    public List<IStatusEffect> StatusEffects { get; set; } = new();

    public string Name { get; set; } = "Imp";
    public int Alignment { get; set; } = 1;
    public int Initiative { get; set; } = 1;
    public ClampedInt Armor { get; set; } = new(0, 1, 1);
    public ClampedInt Health { get; set; } = new(0, 5, 5);
    public ClampedInt ActionPoints { get; set; } = new(0, 3, 3);
    public ClampedInt MovementPoints { get; set; } = new(0, 12, 12);
    public List<ISkill> Skills { get; set; } = new() {
        new Slash(),
        new Ignite(),
    };
    public AIProfile Profile => AIProfiles.RANGED;

    public int StunSources { get; set; }
    public Color Color => new Color(.8f, .2f, .0f);
    public char Character => 'I';
}