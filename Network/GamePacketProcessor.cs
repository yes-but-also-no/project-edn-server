using System;
using Sylver.Network.Data;

namespace Network
{
    /// <summary>
    /// Packet processor for the game engine
    /// </summary>
    public class GamePacketProcessor : IPacketProcessor
    {
        /// <summary>
        /// Exteel packet header size
        /// Header will be a single short value
        /// </summary>
        public int HeaderSize => 2;

        /// <inheritdoc />
        public bool IncludeHeader => false;
        
        /// <inheritdoc />
        public int GetMessageLength(byte[] buffer)
        {
            return ((buffer[1] << 8) + buffer[0]) - HeaderSize;
        }

        /// <inheritdoc />
        public INetPacketStream CreatePacket(byte[] buffer) => new GamePacket(buffer);
    }
}