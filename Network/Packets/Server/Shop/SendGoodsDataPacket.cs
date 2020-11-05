using System.Collections.Generic;
using Data.Configuration.Shop;

namespace Network.Packets.Server.Shop
{
    public class SendGoodsDataPacket : IPacketSerializer
    {
        /// <summary>
        /// The goods to send
        /// </summary>
        public List<Good> Goods { get; set; }

        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.SendGoodsData;

        public void Serialize(GamePacket packet)
        {
            packet.Write(Goods.Count); // Number of goods in this packet

            // Write all goods
            foreach (var good in Goods)
            {
                packet.Write(good.Id); // Good id
                packet.Write(good.CreditPrice); // Credit price
                packet.Write(good.CoinPrice); // Coin price
                packet.Write(0); // Unknown
                
                packet.WriteGameString(good.ItemNameCode); // Localization string for good name
                packet.WriteGameString(good.ItemDescCode); // Localization string for good description
                
                packet.Write(good.Templates.Length); // Number of contract options for this good that there is

                // Write all templates
                foreach (var template in good.Templates)
                {
                    packet.Write(0); // Unknown
                    packet.Write((int)good.ProductType); // Product type
                    packet.Write(template);
                    packet.Write((int)good.ContractType); // Contract type
                    packet.Write(good.ContractValue); // Contract value
                    packet.Write(0); // Unknown
                }
            }
        }

    }
}