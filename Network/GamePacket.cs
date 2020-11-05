using System.IO;
using System.Numerics;
using System.Text;
using Network.Packets.Server;
using Sylver.Network.Data;

namespace Network
{
    /// <summary>
    /// Represents a single game packet
    /// </summary>
    public class GamePacket : NetPacketStream
    {
        public const byte NullId = 0xFF;
        
        /// <inheritdoc />
        protected override Encoding ReadEncoding => Encoding.Unicode;

        /// <inheritdoc />
        protected override Encoding WriteEncoding => Encoding.Unicode;
        
        /// <summary>
        /// Gets the GamePacket buffer.
        /// </summary>
        public override byte[] Buffer => BuildPacketBuffer();
        
        /// <summary>
        /// Creates a new GamePacket in write-only mode.
        /// </summary>
        public GamePacket()
        {
            base.Write<ushort>(0);
        }

        /// <summary>
        /// Creates a new GamePacket with a header.
        /// </summary>
        /// <param name="packetHeader"></param>
        public GamePacket(ServerPacketType packetHeader)
            : this()
        {
            WriteHeader(packetHeader);
        }

        /// <summary>
        /// Creates a new GamePacket in read-only mode.
        /// </summary>
        /// <param name="buffer"></param>
        public GamePacket(byte[] buffer)
            : base(buffer)
        {
        }

        /// <summary>
        /// Write packet header.
        /// </summary>
        /// <param name="packetHeader">GamePacket header</param>
        public void WriteHeader(ServerPacketType packetHeader) => Write((byte)packetHeader);
        
        /// <summary>
        /// Builds the packet buffer.
        /// </summary>
        /// <returns></returns>
        private byte[] BuildPacketBuffer()
        {
            long oldPointer = Position;

            Seek(0, SeekOrigin.Begin);
            base.Write((ushort)Length);
            Seek(oldPointer, SeekOrigin.Begin);

            return base.Buffer;
        }
        
        /// <summary>
        /// Reads a vector
        /// </summary>
        /// <returns></returns>
        public Vector3 ReadVector()
        {
            return new Vector3(
                    Read<float>(),
                    Read<float>(),
                    Read<float>()
                );
        }

        /// <summary>
        /// Special override for strings, which use 2 byte lengths instead of 4
        /// and are null terminated
        /// </summary>
        /// <returns></returns>
        public string ReadGameString()
        {
            var stringLength = Read<short>();
            var stringBytes = Read<byte>(stringLength);

            return ReadEncoding.GetString(stringBytes);
        }
        
        /// <summary>
        /// Writes a vector
        /// </summary>
        /// <returns></returns>
        public void ReadVector(Vector3 value)
        {
            Write(value.X);
            Write(value.Y);
            Write(value.Z);
        }
        
        /// <summary>
        /// Special override for strings, which use 2 byte lengths instead of 4
        /// and are null terminated
        /// </summary>
        /// <returns></returns>
        public void WriteGameString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Write<short>(2); // Length
                
                Write<byte>(0); // Null terminator
                Write<byte>(0);
            }
            else
            {
                value += "\0";
                
                // Convert to bytes
                var arr = value.ToCharArray();
                
                // Write the size as a short
                Write<short>((short)(value.Length*2));
                
                // Add to list
                foreach (var c in arr)
                {
                    Write(c);
                }
            }
        }
    }
}