using System.Collections.Generic;
using Data.Model.Items;
using Sylver.Network.Data;

namespace Network.Packets.Server.Inventory
{
    public class InventoryPartsPacket : IPacketSerializer
    {
        /// <summary>
        /// Parts to send in this packet
        /// </summary>
        public List<PartRecord> Parts { get; set; }

        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.InventoryParts;

        public void Serialize(GamePacket packet)
        {
            packet.Write(Parts.Count); // Size of array

            foreach (var part in Parts)
            {
                packet.WritePartInfo(part);
            }
        }

    }
}