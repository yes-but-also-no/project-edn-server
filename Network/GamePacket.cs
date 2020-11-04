using System.IO;
using System.Text;
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
        public GamePacket(object packetHeader)
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
        public void WriteHeader(object packetHeader) => Write((byte)packetHeader);
        
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
    }
}