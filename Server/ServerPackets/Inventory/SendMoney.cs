using Data.Model;
using Data.Model.Items;

namespace GameServer.ServerPackets.Inventory
{
    /// <summary>
    /// Sends the users money and coins
    /// </summary>
    public class SendMoney : ServerInventoryBasePacket
    {
        public SendMoney(ExteelUser user) : base(user)
        {
            HighPriority = true;
        }
        
        public override string GetType()
        {
            // Not sure exact packet name in client
            return "INV_SEND_MONEY";
        }

        public override byte GetId()
        {
            return 0x1d;
        }

        protected override void WriteImpl()
        {
            WriteUInt(Inventory.User.Coins);
            WriteUInt(Inventory.User.Credits);
        }

    }
}