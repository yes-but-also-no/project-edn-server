using Data.Configuration;
using GameServer.ServerPackets.Shop;

namespace GameServer.ClientPackets.Shop
{
    public class RequestGoodsData : ClientBasePacket
    {
        /// <summary>
        /// Called when the client needs to download shop data
        /// Totally unknown
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        public RequestGoodsData(byte[] data, GameSession client) : base(data, client)
        {
        }

        public override string GetType()
        {
            return "REQ_GOODS_DATA";
        }

        protected override void RunImpl()
        {
            // TODO: This should run in another thread?
            var client = GetClient();
            
            client.SendPacket(new SendGoodsDataStart());
            
            client.SendPacket(new SendGoodsData(ShopDataReader.Set));
            
            client.SendPacket(new SendGoodsData(ShopDataReader.Head));
            client.SendPacket(new SendGoodsData(ShopDataReader.Chest));
            client.SendPacket(new SendGoodsData(ShopDataReader.Arm));
            client.SendPacket(new SendGoodsData(ShopDataReader.Leg));
            client.SendPacket(new SendGoodsData(ShopDataReader.Booster));
            
            client.SendPacket(new SendGoodsData(ShopDataReader.Weapon));
            
            client.SendPacket(new SendGoodsData(ShopDataReader.Code));
            client.SendPacket(new SendGoodsData(ShopDataReader.Etc));
            
            client.SendPacket(new SendGoodsDataEnd());
        }
    }
}