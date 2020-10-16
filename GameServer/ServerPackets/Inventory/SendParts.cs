using System.Linq;
using Colorful;
using Data.Model;
using Data.Model.Items;

namespace GameServer.ServerPackets.Inventory
{
    /// <summary>
    /// Sends all the parts (and other things?) in the users inventory
    /// </summary>
    public class SendParts : ServerInventoryBasePacket
    {
        public SendParts(ExteelUser user) : base(user)
        {
            HighPriority = true;
        }

        public override string GetType()
        {
            return "INV_SEND_PARTS";
        }

        public override byte GetId()
        {
            return 0x20;
        }

        protected override void WriteImpl()
        {
            var parts = Inventory.Parts.Where(p => p.IsUnitPart).ToList();
            
            WriteInt(parts.Count);

            foreach (var part in parts)
            {
                this.WritePartInfo(part);
            }
        }
    }
}