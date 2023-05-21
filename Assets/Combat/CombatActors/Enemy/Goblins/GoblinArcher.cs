using System;
using System.Collections.Generic;
using UnityEngine;

public class GoblinArcher : ICombatActor, AICombatActor
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public Vector2Int Position { get; set; }
    public List<IStatusEffect> StatusEffects { get; set; } = new();

    public string Name { get; set; } = "Goblin Archer";
    public int Alignment { get; set; } = 1;
    public int Initiative { get; set; } = 1;
    public ClampedInt Armor { get; set; } = new(0, 2, 2);
    public ClampedInt Health { get; set; } = new(0, 3, 3);
    public ClampedInt ActionPoints { get; set; } = new(0, 3, 3);
    public ClampedInt MovementPoints { get; set; } = new(0, 4, 4);
    public List<ISkill> Skills { get; set; } = new() {
        new ShootArrow() {Damage = 1},
        new TacticalRetreat() {Range = 5}
    };
    public AIProfile Profile => AIProfiles.RANGED;

    public int StunSources { get; set; }
    public Color Color => new Color(0.4f, .8f, .1f);
    public char Character => 'A';
}