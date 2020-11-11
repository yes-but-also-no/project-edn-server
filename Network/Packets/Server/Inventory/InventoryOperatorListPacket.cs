using System.Collections.Generic;
using Data.Model.Items;
using Sylver.Network.Data;

namespace Network.Packets.Server.Inventory
{
    public class InventoryOperatorListPacket : IPacketSerializer
    {
        /// <summary>
        /// The list of operators
        /// </summary>
        public List<PartRecord> Operators { get; set; }
        
        /// <summary>
        /// The currently selected operator
        /// </summary>
        public int SelectedOperator { get; set; }

        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.InventoryOperatorList;

        public void Serialize(GamePacket packet)
        {
            packet.Write(Operators.Count); // Size of array
            
            foreach (var op in Operators)
            {
                packet.Write(op.Id); // ID
                packet.Write(op.TemplateId); // Template Id
                packet.Write(-1); // Remaining durability / time?
                packet.Write(0x14997000); // Max durability? - value from pcap
                packet.Write(2); // Probably contract type - from pcap
                packet.Write(0); // Unknown - from pcap
                packet.Write(op.Id == SelectedOperator ? 1 : 0);
            }
        }

    }
}