using System.Collections.Generic;
using UnityEngine;

public class Pyromancer : PlayableCombatActor
{
    public override List<ISkill> Skills { get; set; } = new() {
        new GreaseBomb(),
        new Ignite(),
        new Flamewall(),
    };
    public override Color Color => new Color(1f, .5f, .2f);
}