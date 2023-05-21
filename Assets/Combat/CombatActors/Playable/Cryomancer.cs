using System.Collections.Generic;
using UnityEngine;

public class Cryomancer : PlayableCombatActor
{
    public override List<ISkill> Skills { get; set; } = new() {
        new IceCarpet(),
        new IceNova(),
        new ShatterIce(),
    };
    public override Color Color => new Color(.2f, .9f, .8f);
}