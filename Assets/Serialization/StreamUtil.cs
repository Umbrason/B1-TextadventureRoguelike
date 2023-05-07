using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PackageSystem
{
    public static class StreamUtil
    {
        #region Integers
        public static void WriteShort(this Stream stream, short value) => stream.Write(BitConverter.GetBytes(value), 0, sizeof(short));
        public static short ReadShort(this Stream stream)
        {
            var bytes = new byte[sizeof(short)];
            stream.Read(bytes, 0, sizeof(short));
            return BitConverter.ToInt16(bytes, 0);
        }

        public static void WriteInt(this Stream stream, int value) => stream.Write(BitConverter.GetBytes(value), 0, sizeof(int));
        public static int ReadInt(this Stream stream)
        {
            var bytes = new byte[sizeof(int)];
            stream.Read(bytes, 0, sizeof(int));
            return BitConverter.ToInt32(bytes, 0);
        }

        public static void WriteLong(this Stream stream, long value) => stream.Write(BitConverter.GetBytes(value), 0, sizeof(long));
        public static long ReadLong(this Stream stream)
        {
            var bytes = new byte[sizeof(long)];
            stream.Read(bytes, 0, sizeof(long));
            return BitConverter.ToInt64(bytes, 0);
        }

        public static void WriteUShort(this Stream stream, ushort value) => stream.Write(BitConverter.GetBytes(value), 0, sizeof(ushort));
        public static ushort ReadUShort(this Stream stream)
        {
            var bytes = new byte[sizeof(ushort)];
            stream.Read(bytes, 0, sizeof(ushort));
            return BitConverter.ToUInt16(bytes, 0);
        }

        public static void WriteUInt(this Stream stream, uint value) => stream.Write(BitConverter.GetBytes(value), 0, sizeof(uint));
        public static uint ReadUInt(this Stream stream)
        {
            var bytes = new byte[sizeof(uint)];
            stream.Read(bytes, 0, sizeof(uint));
            return BitConverter.ToUInt32(bytes, 0);
        }

        public static void WriteULong(this Stream stream, ulong value) => stream.Write(BitConverter.GetBytes(value), 0, sizeof(ulong));
        public static ulong ReadULong(this Stream stream)
        {
            var bytes = new byte[sizeof(ulong)];
            stream.Read(bytes, 0, sizeof(ulong));
            return BitConverter.ToUInt64(bytes, 0);
        }
        #endregion

        #region Floating-Point
        public static void WriteFloat(this Stream stream, float value)
        {
            var bytes = BitConverter.GetBytes(value);
            stream.Write(bytes, 0, bytes.Length);
        }
        public static float ReadFloat(this Stream stream)
        {
            var bytes = new byte[sizeof(float)];
            stream.Read(bytes, 0, sizeof(float));
            return BitConverter.ToSingle(bytes, 0);
        }
        #endregion

        #region GUID
        const int GUID_SIZE = 16;
        public static void WriteGuid(this Stream stream, Guid guid) => stream.Write(guid.ToByteArray(), 0, GUID_SIZE);
        public static Guid ReadGuid(this Stream stream)
        {
            var bytes = new byte[GUID_SIZE];
            stream.Read(bytes, 0, GUID_SIZE);
            return new Guid(bytes);
        }
        #endregion

        #region String
        public static void WriteString(this Stream stream, string value)
        {
            var bytes = Encoding.Unicode.GetBytes(value);
            stream.WriteInt(bytes.Length);
            stream.Write(bytes, 0, bytes.Length);
        }
        public static string ReadString(this Stream stream)
        {
            var length = stream.ReadInt();
            var bytes = new byte[length];
            stream.Read(bytes, 0, length);
            return Encoding.Unicode.GetString(bytes);
        }
        #endregion

        #region Collections
        public static void WriteEnumerable<T>(this Stream stream, IEnumerable<T> enumerable, Action<T> writeElementCallback)
        {
            stream.WriteInt(enumerable.Count());
            foreach (var entry in enumerable)
                writeElementCallback(entry);
        }

        public static T[] ReadEnumerable<T>(this Stream stream, Func<T> readElementCallback)
        {
            var len = stream.ReadInt();
            var array = new T[len];
            for (int i = 0; i < len; i++)
                array[i] = readElementCallback();
            return array;
        }

        public static void WriteDictionary<K, V>(this Stream stream, IReadOnlyDictionary<K, V> dictionary, Action<K> writeKeyCallback, Action<V> writeValueCallback)
            => WriteEnumerable(stream, dictionary, (kv) => { writeKeyCallback(kv.Key); writeValueCallback(kv.Value); });

        public static Dictionary<K, V> ReadDictionary<K, V>(this Stream stream, Func<K> readKeyCallback, Func<V> readValueCallback)
        {
            var pairs = ReadEnumerable<KeyValuePair<K, V>>(stream, () => new KeyValuePair<K, V>(readKeyCallback(), readValueCallback()));
            var dict = new Dictionary<K, V>(pairs.Length);
            foreach (var pair in pairs) dict.Add(pair.Key, pair.Value);
            return dict;
        }
        #endregion

        public static void WriteEnum<T>(this Stream stream, T value) where T : System.Enum
            => stream.WriteString(value.ToString());

        public static T ReadEnum<T>(this Stream stream) where T : System.Enum
            => (T)Enum.Parse(typeof(T), stream.ReadString());

        public static void WriteBool(this Stream stream, bool value)
            => stream.WriteByte((byte)(value ? 1 : 0));

        public static bool ReadBool(this Stream stream)
            => stream.ReadByte() > 0;

        public static void WriteIBinarySerializable(this Stream stream, IBinarySerializable serializable)
            => stream.WriteEnumerable(serializable.Bytes, stream.WriteByte);
        public static void ReadIBinarySerializable(this Stream stream, ref IBinarySerializable serializable)
            => serializable.Bytes = stream.ReadEnumerable<byte>(() => (byte)stream.ReadByte());

        public static byte[] GetAllBytes(this Stream stream)
        {
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);
            stream.Close();
            return bytes;
        }

        public static void WriteVector3(this Stream stream, Vector3 vector3)
        {
            stream.WriteFloat(vector3.x);
            stream.WriteFloat(vector3.y);
            stream.WriteFloat(vector3.z);
        }

        public static Vector3 ReadVector3(this Stream stream)
        {
            return new Vector3(stream.ReadFloat(), stream.ReadFloat(), stream.ReadFloat());
        }

        public static void WriteVector3Int(this Stream stream, Vector3Int vector3)
        {
            stream.WriteInt(vector3.x);
            stream.WriteInt(vector3.y);
            stream.WriteInt(vector3.z);
        }

        public static Vector3Int ReadVector3Int(this Stream stream)
        {
            return new Vector3Int(stream.ReadInt(), stream.ReadInt(), stream.ReadInt());
        }

        public static void WriteVector2(this Stream stream, Vector2 vector2)
        {
            stream.WriteFloat(vector2.x);
            stream.WriteFloat(vector2.y);
        }

        public static Vector2 ReadVector2(this Stream stream)
        {
            return new Vector2(stream.ReadFloat(), stream.ReadFloat());
        }

        public static void WriteVector2Int(this Stream stream, Vector2Int vector2)
        {
            stream.WriteInt(vector2.x);
            stream.WriteInt(vector2.y);
        }

        public static Vector2Int ReadVector2Int(this Stream stream)
        {
            return new Vector2Int(stream.ReadInt(), stream.ReadInt());
        }
    }
}