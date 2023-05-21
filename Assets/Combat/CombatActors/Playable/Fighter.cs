using System.Collections.Generic;
using UnityEngine;

public class Fighter : PlayableCombatActor
{
    public override List<ISkill> Skills { get; set; } = new() {
        new Slash(),
        new ArmorUp()
    };
    public override Color Color => new Color(.4f, .4f, .4f);
}
