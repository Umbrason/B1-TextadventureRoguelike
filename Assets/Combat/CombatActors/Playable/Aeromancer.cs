
using System.Collections.Generic;
using UnityEngine;

public class Aeromancer : PlayableCombatActor
{
    public override List<ISkill> Skills { get; set; } = new() {
        new Slash(),
        new Gust(),
        new Teleport(),
    };
    public override Color Color => new Color(.9f, .9f, .8f);
}
