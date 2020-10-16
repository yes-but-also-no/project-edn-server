using System.Linq;
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
            var parts = Inventory.Parts.Where(p => p.IsSpecialPart).ToList();
            
            WriteInt(parts.Count);

            foreach (var part in parts)
            {
                WriteInt(part.Id); // ID
                WriteUInt(part.TemplateId); // Template Id
                WriteInt(4000); // Unknown - From pcap. Maybe sell price?
                WriteInt(-1); // Remaining durability / time?
                WriteInt(0x14997000); // Max durability? - value from pcap
                WriteInt(2); // Probably contract type - from pcap
                WriteInt(0); // Unknown - from pcap
            }
        }
    }
}