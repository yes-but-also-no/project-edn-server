using System.Collections.Generic;
using Data.Model.Items;
using Sylver.Network.Data;

namespace Network.Packets.Server.Inventory
{
    public class InventorySpecialItemsPacket : IPacketSerializer
    {
        /// <summary>
        /// The special items to write
        /// </summary>
        public List<PartRecord> SpecialItems { get; set; }
        
        /// <summary>
        /// The repair points of this user
        /// </summary>
        public uint RepairPoints { get; set; }

        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.InventorySpecialItems;

        public void Serialize(GamePacket packet)
        {
            packet.Write(SpecialItems.Count); // Size of array

            foreach (var item in SpecialItems)
            {
                packet.Write(item.Id);
                packet.Write(item.TemplateId);
                packet.Write(RepairPoints); // For some reason the repair points is sent here?
                
                packet.Write(-1); // Remaining durability / time?
                packet.Write(0x14997000); // Max durability? - value from pcap
                packet.Write(2); // Probably contract type - from pcap
                packet.Write(0); // Unknown - from pcap
            }
        }

    }
}