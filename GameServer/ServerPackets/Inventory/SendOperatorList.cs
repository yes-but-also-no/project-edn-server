using Data.Model;

namespace GameServer.ServerPackets.Inventory
{
    /// <summary>
    /// Sends a list of operators... that the user has? or for sale. not sure
    /// </summary>
    public class SendOperatorList : ServerInventoryBasePacket
    {
        public SendOperatorList(ExteelUser user) : base(user)
        {
            HighPriority = true;
        }

        public override string GetType()
        {
            return "INV_OP_ITEM_LIST";
        }

        public override byte GetId()
        {
            return 0x24;
        }

        protected override void WriteImpl()
        {
            WriteInt(7); // Size
            
            WriteInt(1); // Id?
            WriteInt(6001); // Template
            WriteInt(-1); // Time - seconds, -1 for unlimited
            WriteInt(0); // Unknown
            WriteInt(0); // Unknown
            WriteInt(0); // Unknown
            WriteInt(1); // Selected?

            for (var i = 2; i <= 7; i++)
            {

                WriteInt(i); // Id?
                WriteInt(6000 + i); // Template
                WriteInt(-1); // Time - seconds, -1 for unlimited
                WriteInt(0); // Unknown
                WriteInt(0); // Unknown
                WriteInt(0); // Unknown
                WriteInt(0); // Unknown
            }
        }
    }
}