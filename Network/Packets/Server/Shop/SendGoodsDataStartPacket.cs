using Sylver.Network.Data;

namespace Network.Packets.Server.Shop
{
    public class SendGoodsDataStartPacket : IPacketSerializer
    {

        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.SendGoodsDataStart;

        public void Serialize(GamePacket packet)
        {
            packet.Write(9); // This is hard coded as 9, which is the number of goods categories there are
        }

    }
}