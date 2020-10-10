using System;
using System.IO;

namespace GameServer.GeoEngine
{
    public static class ByteReader
    {
        private const byte xorKey = 0x42;

        public static short GetShort(BinaryReader reader)
        {
            return BitConverter.ToInt16(ReadBytes(reader, 2));
        }
        
        public static int GetInt(BinaryReader reader)
        {
            return BitConverter.ToInt32(ReadBytes(reader, 4));
        }
        
        public static short GetByte(BinaryReader reader)
        {
            return ReadBytes(reader, 1)[0];
        }
        
        public static float GetSingle(BinaryReader reader)
        {
            return BitConverter.ToSingle(ReadBytes(reader, 4));
        }
        
        private static byte[] ReadBytes(BinaryReader reader, int count)
        {
            var data = reader.ReadBytes(count);

            for (var i = 0; i < data.Length; i++)
            {
                data[i] ^= xorKey;
            }
            
            return data;
        }
    }
}