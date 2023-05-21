using System.Collections.Generic;

public interface IObjectCombatActor : ICombatActor
{
    int ICombatActor.Initiative { get => -1; set { } }
    int IReadOnlyCombatActor.Initiative => -1;
    ClampedInt ICombatActor.ActionPoints { get => new(0, 0, 0); set { } }
    ClampedInt ICombatActor.MovementPoints { get => new(0, 0, 0); set { } }
    List<ISkill> ICombatActor.Skills { get => new(); set { } }
    IReadOnlyList<IReadOnlySkill> IReadOnlyCombatActor.Skills { get => new List<ISkill>(); }
    int IReadOnlyCombatActor.Alignment => Alignment;
}