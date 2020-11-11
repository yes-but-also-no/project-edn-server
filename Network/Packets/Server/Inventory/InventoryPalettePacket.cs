using Data.Model;
using Sylver.Network.Data;

namespace Network.Packets.Server.Inventory
{
    public class InventoryPalettePacket : IPacketSerializer
    {
        /// <summary>
        /// The unit this palette packet is for
        /// </summary>
        public UnitRecord Unit { get; set; }

        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.InventoryPalette;

        public void Serialize(GamePacket packet)
        {
            packet.Write(Unit.Id);
            
            packet.Write(4); // Slot count?
            
            // Below can probably be clean up 
            var count = 0;

            if (Unit.Skill1Id != null)
                count++;
            
            if (Unit.Skill2Id != null)
                count++;
            
            if (Unit.Skill3Id != null)
                count++;
            
            if (Unit.Skill4Id != null)
                count++;
            
            
            packet.Write(count); // Code count?
            
            if (Unit.Skill1Id != null)
                packet.Write(Unit.Skill1Id.Value);
            
            if (Unit.Skill2Id != null)
                packet.Write(Unit.Skill2Id.Value);
            
            if (Unit.Skill3Id != null)
                packet.Write(Unit.Skill3Id.Value);
            
            if (Unit.Skill4Id != null)
                packet.Write(Unit.Skill4Id.Value);
        }

    }
}