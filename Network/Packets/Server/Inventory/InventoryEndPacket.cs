using Sylver.Network.Data;

namespace Network.Packets.Server.Inventory
{
    public class InventoryEndPacket : IPacketSerializer
    {
        /// <summary>
        /// How much inventory has been used
        /// </summary>
        public int InventoryUsed { get; set; }
        
        /// <summary>
        /// The max inventory size
        /// </summary>
        public int InventoryMax { get; set; }

        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.InventoryEnd;

        public void Serialize(GamePacket packet)
        {
            packet.Write(InventoryUsed);
            packet.Write(InventoryMax);
            
            packet.Write(0); // Unknown
            packet.Write(0); // Unknown
            packet.Write(0); // Unknown
            packet.Write(0); // Unknown
            
            // FArray - Unknown
            packet.Write(0); // Unknown - Size?
            
            // FArray - Unknown
            packet.Write(0); // Unknown - Size?
        }

    }
}