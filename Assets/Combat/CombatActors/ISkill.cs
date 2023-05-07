
using System;
using System.Collections.Generic;
using UnityEngine;

public interface ISkill : IBinarySerializable
{
    public int Cooldown { get; set; }
    public ITargetSelector[] TargetSelectors { get; }
    public interface ITargetSelector
    {
        object Target { get; }
        Type type { get; }
    }
    public bool CanUse(CombatState combatState, ISkillUser user);
    public void Execute(CombatState combatState, ISkillUser user, params object[] parameters);
}

public interface ISkillUser
{
    public Guid Guid { get; }
    public int Alignment { get; }
    public int AP { get; }
    public Vector2Int Position { get; }
    public IReadOnlyList<ISkill> Skills { get; }

}