using System;
using System.Linq;
using Data.Model;
using Data.Model.Items;

namespace GameServer.ServerPackets.Inventory
{
    /// <summary>
    /// This looks like a packet for skills in the users inventory
    /// </summary>
    public class SendCodeList : ServerInventoryBasePacket
    {
        public SendCodeList(ExteelUser user) : base(user)
        {
            HighPriority = true;
        }

        public override string GetType()
        {
            return "INV_CODE_LIST";
        }

        public override byte GetId()
        {
            return 0x1f;
        }

        protected override void WriteImpl()
        {
            var codes = Inventory.Parts.Where(p => p.IsCode).ToList();
            
            // All unknown
            WriteInt(0); // Always zero?
            
            WriteInt(codes.Count); // Array size

            foreach (var code in codes)
            {
                // Array item
                WriteInt(code.Id); // Id
                WriteUInt(code.TemplateId); // Template
                WriteInt(0); // Unknown
                WriteInt(0); // Unknown
                WriteInt(0); // Unknown
                WriteInt(10000); // Expiry time / current durability
                WriteInt(10000); // Max Durability - Value from server packet capture
                WriteInt(1); // If 1, durability, if 2, expired?
                WriteInt(0); // Unknown
            }
           
        }
    }
}