using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCombatActor : ICombatActor
{
    public string Name { get; set; }
    public Guid Guid { get; set; }
    public int Alignment { get; set; }
    public Vector2Int Position { get; set; }
    public ClampedInt Health { get; set; }
    public ClampedInt ActionPoints { get; set; } = new(0, 6, 6);
    public ClampedInt MovementPoints { get; set; } = new(0, 10, 10);
    public int Initiative { get; set; }
    public List<ISkill> Skills { get; set; } = new();
    public List<IStatusEffect> StatusEffects { get; set; } = new();
    public IBinarySerializable CustomState => null;    
}
