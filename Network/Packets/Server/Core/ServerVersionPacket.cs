using Sylver.Network.Data;

namespace Network.Packets.Server.Core
{
    /// <summary>
    /// This packet is mostly unknown. Values are hard coded because they are known to work. 
    /// </summary>
    public class ServerVersionPacket : IPacketSerializer
    {

        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.ServerVersion;

        public void Serialize(GamePacket packet)
        {
            packet.Write(0); // Unknown
            packet.Write(0); // Unknown
            packet.Write(3459107938); // Unknown
            packet.Write(0); // Unknown
        }

    }
}