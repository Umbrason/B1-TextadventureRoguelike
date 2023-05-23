using System.Collections.Generic;
using UnityEngine;

public class Cryomancer : PlayableCombatActor
{
    public override List<ISkill> Skills { get; set; } = new() {
        new Slash(),
        new IceCarpet(),
        new IceNova(),
    };
    public override Color Color => new Color(.2f, .9f, .8f);
}