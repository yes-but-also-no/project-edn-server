using Data.Model;
using Data.Model.Items;

namespace GameServer.ServerPackets.Inventory
{
    /// <summary>
    /// Sends the users money and coins
    /// </summary>
    public class SendMoneySynced : ServerInventoryBasePacket
    {
        public SendMoneySynced(ExteelUser user) : base(user) {}
        
        public override string GetType()
        {
            return "MONEY_SYNCED";
        }

        public override byte GetId()
        {
            return 0x11;
        }

        protected override void WriteImpl()
        {
            WriteUInt(Inventory.User.Credits);
            WriteUInt(Inventory.User.Coins);
        }

    }
}