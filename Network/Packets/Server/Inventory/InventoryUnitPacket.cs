using Data.Model;
using Sylver.Network.Data;

namespace Network.Packets.Server.Inventory
{
    public class InventoryUnitPacket : IPacketSerializer
    {

        /// <summary>
        /// The user id who owns this unit
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// The unit record
        /// </summary>
        public UnitRecord Unit { get; set; }
        
        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.InventoryUnitInfo;

        public void Serialize(GamePacket packet)
        {
            packet.Write(UserId);
            packet.Write(Unit.Id);
            
            packet.WriteGameString(Unit.Name);
            
            packet.Write(0); // Slot position? (1st, 2nd, etc)
            packet.Write(0); // Unit HP?
            
            packet.WritePartInfo(Unit.Head);
            packet.WritePartInfo(Unit.Chest);
            packet.WritePartInfo(Unit.Arms);
            packet.WritePartInfo(Unit.Legs);
            packet.WritePartInfo(Unit.Backpack);
            
            packet.WritePartInfo(Unit.WeaponSet1Left);
            packet.WritePartInfo(Unit.WeaponSet1Right);
            
            packet.WritePartInfo(Unit.WeaponSet2Left);
            packet.WritePartInfo(Unit.WeaponSet2Right);
        }

    }
}