using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;

public interface ICombatActor : IReadOnlyCombatActor, IBinarySerializable
{
    public new Guid Guid { get; set; }
    public new Vector2Int Position { get; set; }
    public new List<IStatusEffect> StatusEffects { get; set; }

    #region Statblock
    public new string Name { get; set; }
    public new int Alignment { get; set; }
    public new int Initiative { get; set; }
    public new ClampedInt Armor { get; set; }
    public new ClampedInt Health { get; set; }
    public new ClampedInt ActionPoints { get; set; }
    public new ClampedInt MovementPoints { get; set; }
    public new List<ISkill> Skills { get; set; }
    #endregion


    byte[] IBinarySerializable.ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteGuid(Guid);
            stream.WriteVector2Int(Position);
            stream.WriteEnumerable(StatusEffects, stream.WriteIBinarySerializable);
            stream.WriteString(Name);
            stream.WriteInt(Alignment);
            stream.WriteInt(Initiative);
            stream.WriteClampedInt(Armor);
            stream.WriteClampedInt(Health);
            stream.WriteClampedInt(ActionPoints);
            stream.WriteClampedInt(MovementPoints);
            stream.WriteEnumerable(Skills, stream.WriteIBinarySerializable);
            return stream.GetAllBytes();
        }
        set
        {
            var stream = new MemoryStream(value);
            Guid = stream.ReadGuid();
            Position = stream.ReadVector2Int();
            StatusEffects = stream.ReadEnumerable(stream.ReadIBinarySerializable<IStatusEffect>).ToList();
            Name = stream.ReadString();
            Alignment = stream.ReadInt();
            Initiative = stream.ReadInt();
            Armor = stream.ReadClampedInt();
            Health = stream.ReadClampedInt();
            ActionPoints = stream.ReadClampedInt();
            MovementPoints = stream.ReadClampedInt();
            Skills = stream.ReadEnumerable(stream.ReadIBinarySerializable<ISkill>).ToList();
        }
    }

    IReadOnlyClampedInt IReadOnlyCombatActor.Armor => Armor;
    IReadOnlyClampedInt IReadOnlyCombatActor.Health => Health;
    IReadOnlyClampedInt IReadOnlyCombatActor.ActionPoints => ActionPoints;
    IReadOnlyClampedInt IReadOnlyCombatActor.MovementPoints => MovementPoints;
    IReadOnlyList<IReadOnlySkill> IReadOnlyCombatActor.Skills => Skills;
    IReadOnlyList<IReadOnlyStatusEffect> IReadOnlyCombatActor.StatusEffects => StatusEffects;
}

public interface IReadOnlyCombatActor
{
    public string Name { get; }
    public Guid Guid { get; }
    public int Alignment { get; }
    public Vector2Int Position { get; }

    #region Statblock
    public IReadOnlyClampedInt Armor { get; }
    public IReadOnlyClampedInt Health { get; }
    public IReadOnlyClampedInt ActionPoints { get; }
    public IReadOnlyClampedInt MovementPoints { get; }
    public int Initiative { get; }
    #endregion

    public IReadOnlyList<IReadOnlySkill> Skills { get; }
    public IReadOnlyList<IReadOnlyStatusEffect> StatusEffects { get; }
}
