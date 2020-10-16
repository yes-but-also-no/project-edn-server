using System.Linq;
using Data.Model;

namespace GameServer.ServerPackets.Inventory
{
    /// <summary>
    /// Sends a list of operators... that the user has? or for sale. not sure
    /// </summary>
    public class SendOperatorList : ServerInventoryBasePacket
    {
        private readonly int _selectedOperator;
        
        public SendOperatorList(ExteelUser user) : base(user)
        {
            HighPriority = true;
            _selectedOperator = user.OperatorId;
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
            var parts = Inventory.Parts.Where(p => p.IsOperator).ToList();
            
            WriteInt(parts.Count);
            
            foreach (var part in parts)
            {
                WriteInt(part.Id); // ID
                WriteUInt(part.TemplateId); // Template Id
                WriteInt(-1); // Remaining durability / time?
                WriteInt(0x14997000); // Max durability? - value from pcap
                WriteInt(2); // Probably contract type - from pcap
                WriteInt(0); // Unknown - from pcap
                WriteInt(part.Id == _selectedOperator ? 1 : 0);
            }
            
            // WriteInt(1); // Id?
            // WriteInt(6001); // Template
            // WriteInt(-1); // Time - seconds, -1 for unlimited
            // WriteInt(0); // Unknown
            // WriteInt(0); // Unknown
            // WriteInt(0); // Unknown
            // WriteInt(1); // Selected?

            // for (var i = 2; i <= 7; i++)
            // {
            //
            //     WriteInt(i); // Id?
            //     WriteInt(6000 + i); // Template
            //     WriteInt(-1); // Time - seconds, -1 for unlimited
            //     WriteInt(0); // Unknown
            //     WriteInt(0); // Unknown
            //     WriteInt(0); // Unknown
            //     WriteInt(0); // Unknown
            // }
        }
    }
}