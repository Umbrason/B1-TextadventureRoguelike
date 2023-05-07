using System;
using System.IO;
using PackageSystem;
using UnityEngine;

public interface ICombatActor : IReadOnlyCombatActor, ISkillUser, IBinarySerializable
{
    public void NotifyTurnStart(TurnInfo turnInfo);
    public new Guid Guid { get; set; }
    public new int Alignment { get; set; }
    public ClampedInt Health { get; set; }
    public int Initiative { get; }
    public new Vector2Int Position { get; set; }
    public IBinarySerializable State { get; }

    byte[] IBinarySerializable.Bytes
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

public interface IReadOnlyCombatActor
{
    public Guid Guid { get; }
    public int Alignment { get; }
    public Vector2Int Position { get; }
}