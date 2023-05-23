using System.Collections.Generic;
using UnityEngine;

public class Necromancer : PlayableCombatActor
{
    public override List<ISkill> Skills { get; set; } = new() {
        new Slash(),
        new LifeDrain(),
        new NecroticTouch()
    };
    public override Color Color => new Color(.6f, .1f, .2f);
}