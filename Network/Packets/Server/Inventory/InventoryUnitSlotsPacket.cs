using Sylver.Network.Data;

namespace Network.Packets.Server.Inventory
{
    public class InventoryUnitSlotsPacket : IPacketSerializer
    {
        /// <summary>
        /// Number of unit slots this user has
        /// </summary>
        public uint Slots { get; set; }

        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.InventoryUnitSlots;

        public void Serialize(GamePacket packet)
        {
            packet.Write(Slots);
        }

    }
}