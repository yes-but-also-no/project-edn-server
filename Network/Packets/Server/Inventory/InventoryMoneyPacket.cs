using Sylver.Network.Data;

namespace Network.Packets.Server.Inventory
{
    public class InventoryMoneyPacket : IPacketSerializer
    {

        /// <summary>
        /// Users Coins
        /// </summary>
        public uint Coins { get; set; }

        /// <summary>
        /// The users credits
        /// </summary>
        public uint Credits { get; set; }

        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.InventoryMoney;

        public void Serialize(GamePacket packet)
        {
            packet.Write(Coins);
            packet.Write(Credits);
        }

    }
}