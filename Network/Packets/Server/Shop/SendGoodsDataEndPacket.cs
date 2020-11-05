using Sylver.Network.Data;

namespace Network.Packets.Server.Shop
{
    public class SendGoodsDataEndPacket : IPacketSerializer
    {

        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.SendGoodsDataEnd;

        public void Serialize(GamePacket packet)
        {
        }

    }
}