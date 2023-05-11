using System;
using System.Collections.Generic;
using System.IO;
using PackageSystem;
using UnityEngine;

public interface ICombatActor : IBinarySerializable
{
    public string Name { get; set; }
    public Guid Guid { get; set; }
    public int Alignment { get; set; }
    public Vector2Int Position { get; set; }

    #region Statblock
    public ClampedInt Health { get; set; }
    public ClampedInt ActionPoints { get; set; }
    public ClampedInt MovementPoints { get; set; }
    public int Initiative { get; }
    #endregion

    public List<ISkill> Skills { get; }
    public List<IStatusEffect> StatusEffects { get; }

    byte[] IBinarySerializable.ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteGuid(Guid);
            stream.WriteInt(Alignment);
            stream.WriteVector2Int(Position);
            return stream.GetAllBytes();
        }
        set
        {
            var stream = new MemoryStream(value);
            Guid = stream.ReadGuid();
            Alignment = stream.ReadInt();
            Position = stream.ReadVector2Int();
        }
    }
}