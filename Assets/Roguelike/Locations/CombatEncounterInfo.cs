using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CombatEncounterInfo : IBinarySerializable
{
    public CombatEncounterInfo(RoomInfo room, ICombatActor[] enemies, Vector2Int[] allyStartPositions, Dictionary<Vector2Int, ITileModifier> tileModifiers)
    {
        this.room = room;
        this.enemies = enemies;
        this.allyStartPositions = allyStartPositions;
        this.tileModifiers = tileModifiers;
    }

    public ICombatActor[] enemies;
    public RoomInfo room;
    public Vector2Int[] allyStartPositions;
    public Dictionary<Vector2Int, ITileModifier> tileModifiers;

    public byte[] ByteData
    {
        get
        {
            var stream = new MemoryStream();
            stream.WriteEnumerable(enemies, stream.WriteIBinarySerializable);
            stream.WriteIBinarySerializable(room);
            stream.WriteEnumerable(allyStartPositions, stream.WriteVector2Int);
            stream.WriteDictionary(tileModifiers, stream.WriteVector2Int, stream.WriteIBinarySerializable);
            return stream.GetAllBytes();
        }
        set
        {
            var stream = new MemoryStream(value);
            enemies = stream.ReadEnumerable(stream.ReadIBinarySerializable<ICombatActor>);
            room = stream.ReadIBinarySerializable<RoomInfo>();
            allyStartPositions = stream.ReadEnumerable(stream.ReadVector2Int);
            tileModifiers = stream.ReadDictionary(stream.ReadVector2Int, stream.ReadIBinarySerializable<ITileModifier>);
        }
    }
}