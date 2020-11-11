using Sylver.Network.Data;

namespace Network.Packets.Server.Inventory
{
    public class InventoryDefaultUnitPacket : IPacketSerializer
    {

        /// <summary>
        /// The default unit id for this user
        /// </summary>
        public int DefaultUnitId { get; set; }
        
        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.InventoryDefaultUnit;

        public void Serialize(GamePacket packet)
        {
            packet.Write(DefaultUnitId);
        }

    }
}