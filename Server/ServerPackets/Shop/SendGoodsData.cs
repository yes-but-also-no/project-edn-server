using System.Collections.Generic;
using Data.Configuration.Shop;

namespace GameServer.ServerPackets.Shop
{
    /// <summary>
    /// Represents a single "chunk" or group of goods items
    /// This is pretty much all unknown
    /// </summary>
    public class SendGoodsData : ServerBasePacket
    {
        private readonly List<Good> _goods;
        
        public SendGoodsData(List<Good> goods)
        {
            _goods = goods;
        }
        
        public override string GetType()
        {
            return "SEND_GOODS_DATA";
        }

        public override byte GetId()
        {
            return 0x91;
        }

        protected override void WriteImpl()
        {           
            // TODO: Figure out this packet
            WriteInt(_goods.Count); // Number of goods in this packet

            foreach (var good in _goods)
            {
                WriteUInt(good.Id);
                WriteUInt(good.CreditPrice);
                WriteUInt(good.CoinPrice);
                WriteUInt(0); // Unknown
                
                WriteString(good.ItemNameCode);
                WriteString(good.ItemDescCode);
                
                WriteInt(good.Templates.Length);

                foreach (var template in good.Templates)
                {
                    WriteInt(0); // Unknown
                    WriteInt((int)good.ProductType);
                    WriteUInt(template);
                    WriteInt((int)good.ContractType);
                    WriteUInt(good.ContractValue);
                    WriteInt(0); // Unknown
                }
            }
        }
    }
}