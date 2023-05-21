using System;
using System.Collections.Generic;
using UnityEngine;

public class RaisedCorpse : ICombatActor, AICombatActor
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public Vector2Int Position { get; set; }
    public List<IStatusEffect> StatusEffects { get; set; } = new();

    public string Name { get; set; } = "Raised Corpse";
    public int Alignment { get; set; } = 1;
    public int Initiative { get; set; } = 3;
    public ClampedInt Armor { get; set; } = new(0, 4, 4);
    public ClampedInt Health { get; set; } = new(0, 10, 10);
    public ClampedInt ActionPoints { get; set; } = new(0, 3, 3);
    public ClampedInt MovementPoints { get; set; } = new(0, 3, 3);
    public int StunSources { get; set; }
    public List<ISkill> Skills { get; set; } = new() {
        new CripplingBlow()
    };
    public AIProfile Profile => AIProfiles.MELEE;

    public Color Color => new Color(.3f, .5f, .25f);
    public char Character => 'C';
}
