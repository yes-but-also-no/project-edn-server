using System.Collections.Generic;
using Data.Model.Items;
using Sylver.Network.Data;

namespace Network.Packets.Server.Inventory
{
    public class InventoryCodeListPacket : IPacketSerializer
    {
        /// <summary>
        /// The special items to write
        /// </summary>
        public List<PartRecord> Codes { get; set; }

        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.InventoryCodeList;

        public void Serialize(GamePacket packet)
        {
            packet.Write(0); // Unknown - Always zero?
            
            packet.Write(Codes.Count); // Size of array

            foreach (var code in Codes)
            {
                packet.Write(code.Id); // Id
                packet.Write(code.TemplateId); // Template
                packet.Write(0); // Unknown
                packet.Write(0); // Unknown
                packet.Write(0); // Unknown
                packet.Write(10000); // Expiry time / current durability
                packet.Write(10000); // Max Durability - Value from server packet capture
                packet.Write(1); // If 1, durability, if 2, expired?
                packet.Write(0); // Unknown
            }
        }

    }
}