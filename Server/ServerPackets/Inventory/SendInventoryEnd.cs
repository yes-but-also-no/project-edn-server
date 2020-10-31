using Data.Model;

namespace GameServer.ServerPackets.Inventory
{
    /// <summary>
    /// Sends when all other inventory packets have been transmitted
    /// </summary>
    public class SendInventoryEnd : ServerInventoryBasePacket
    {
        public SendInventoryEnd(ExteelUser user) : base(user)
        {
            HighPriority = true;
        }

        public override string GetType()
        {
            return "INV_END";
        }

        public override byte GetId()
        {
            return 0x26;
        }

        protected override void WriteImpl()
        {
            WriteInt(Inventory.InventoryUsed);
            WriteUInt(Inventory.InventorySize);
            
            WriteInt(0); // Unknown
            WriteInt(0); // Unknown
            WriteInt(0); // Unknown
            WriteInt(0); // Unknown
            
            // FArray - Unknown
            WriteInt(0); // Unknown - Size?
            
            // FArray - Unknown
            WriteInt(0); // Unknown - Size?
        }
    }
}