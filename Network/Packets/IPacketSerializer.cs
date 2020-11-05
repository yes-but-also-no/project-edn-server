using Network.Packets.Server;
using Sylver.Network.Data;

namespace Network.Packets
{
    /// <summary>
    /// Provides an interface to serialize an object.
    /// </summary>
    public interface IPacketSerializer
    {
        /// <summary>
        /// The server packet type for this packet
        /// </summary>
        ServerPacketType PacketType { get; }
        
        /// <summary>
        /// Serializes the current object.
        /// </summary>
        /// <param name="packet">Packet stream.</param>
        void Serialize(GamePacket packet);
    }
}