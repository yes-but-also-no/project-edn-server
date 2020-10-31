using Data.Model;
using GameServer.ServerPackets.Inventory;

namespace GameServer.ServerPackets.Shop
{
    /// <summary>
    /// This appears to be sent as a result to a purchase
    /// </summary>
    public class BuyListResult : ServerInventoryBasePacket
    {
        public BuyListResult(ExteelUser user) : base(user) {}
        
        public override string GetType()
        {
            return "BUY_LIST_RESULT";
        }

        public override byte GetId()
        {
            return 0x8e;
        }

        protected override void WriteImpl()
        {
            WriteInt(0); // Result?
            WriteUInt(Inventory.User.Coins);
            WriteUInt(Inventory.User.Credits);
        }
    }
}