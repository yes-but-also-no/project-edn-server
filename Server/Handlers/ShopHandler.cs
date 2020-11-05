using Data.Configuration;
using GameServer.New;
using Network;
using Network.Packets.Client;
using Network.Packets.Client.Shop;
using Network.Packets.Server.Shop;

namespace GameServer.Handlers
{
    /// <summary>
    /// This handler takes care of all shop related features in the game
    /// </summary>
    public static class ShopHandler
    {
        /// <summary>
        /// Called when the is requesting goods (shop) data
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(ClientPacketType.RequestGoodsData)]
        public static void OnRequestGoodsData(GameClient client, RequestGoodsDataPacket packet)
        {
            // Send start
            client.Send(PacketFactory.CreatePacket(new SendGoodsDataStartPacket()));
            
            // Send sets
            client.Send(PacketFactory.CreatePacket(new SendGoodsDataPacket
            {
                Goods = ShopDataReader.Set
            }));
            
            // Send heads
            client.Send(PacketFactory.CreatePacket(new SendGoodsDataPacket
            {
                Goods = ShopDataReader.Head
            }));
            
            // Send chests
            client.Send(PacketFactory.CreatePacket(new SendGoodsDataPacket
            {
                Goods = ShopDataReader.Chest
            }));
            
            // Send arms
            client.Send(PacketFactory.CreatePacket(new SendGoodsDataPacket
            {
                Goods = ShopDataReader.Arm
            }));
            
            // Send legs
            client.Send(PacketFactory.CreatePacket(new SendGoodsDataPacket
            {
                Goods = ShopDataReader.Leg
            }));
            
            // Send boosters
            client.Send(PacketFactory.CreatePacket(new SendGoodsDataPacket
            {
                Goods = ShopDataReader.Booster
            }));
            
            // Send weapons
            client.Send(PacketFactory.CreatePacket(new SendGoodsDataPacket
            {
                Goods = ShopDataReader.Weapon
            }));
            
            // Send Codes (skills)
            client.Send(PacketFactory.CreatePacket(new SendGoodsDataPacket
            {
                Goods = ShopDataReader.Code
            }));
            
            // Send etc (paint, expansions, etc)
            client.Send(PacketFactory.CreatePacket(new SendGoodsDataPacket
            {
                Goods = ShopDataReader.Etc
            }));
            
            // Send end
            client.Send(PacketFactory.CreatePacket(new SendGoodsDataEndPacket()));
        }
    }
}