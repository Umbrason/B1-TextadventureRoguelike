
using System.Collections.Generic;
using UnityEngine;

public class Ranger : PlayableCombatActor
{
    public override List<ISkill> Skills { get; set; } = new() {
        new ShootArrow(),
        new TipArrows(),
        new ThrowCaltrops()
    };
    public override Color Color => new Color(.2f, .3f, .2f);
}
