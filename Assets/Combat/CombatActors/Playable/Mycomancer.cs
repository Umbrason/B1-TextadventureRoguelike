using System.Collections.Generic;
using UnityEngine;

public class Mycomancer : PlayableCombatActor
{
    public override List<ISkill> Skills { get; set; } = new() {
        new SummonMushroom(),
        new TriggerMushrooms(),
        new SporeCloud(),
    };
    public override Color Color => new Color(.6f, .9f, .4f);
}
