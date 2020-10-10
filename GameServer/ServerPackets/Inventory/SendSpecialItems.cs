using Data.Model;

namespace GameServer.ServerPackets.Inventory
{
    /// <summary>
    /// Sends special items
    /// Need to investigate further
    /// </summary>
    public class SendSpecialItems : ServerInventoryBasePacket
    {
        public SendSpecialItems(ExteelUser user) : base(user)
        {
            HighPriority = true;
        }

        public override string GetType()
        {
            return "INV_SPECIAL_ITEM_LIST";
        }

        public override byte GetId()
        {
            return 0x25;
        }

        protected override void WriteImpl()
        {
            WriteInt(0); // Unknown - Special item count?
        }
    }
}