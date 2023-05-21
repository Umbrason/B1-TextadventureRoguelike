using System.Collections.Generic;
using UnityEngine;

public class Terramancer : PlayableCombatActor
{
    public override List<ISkill> Skills { get; set; } = new() {
        new HurlBoulders(),
        new RaiseStonewall(),
        new StoneArmor()
    };
    public override Color Color => new Color(.3f, .25f, .2f);
}
