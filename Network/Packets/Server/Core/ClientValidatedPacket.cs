using Sylver.Network.Data;

namespace Network.Packets.Server.Core
{
    public class ClientValidatedPacket : IPacketSerializer
    {

        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.ClientValidated;

        public void Serialize(GamePacket packet)
        {
        }

    }
}