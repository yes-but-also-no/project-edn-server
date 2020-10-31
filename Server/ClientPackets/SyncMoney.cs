using GameServer.ServerPackets.Inventory;

namespace GameServer.ClientPackets
{
    public class SyncMoney : ClientBasePacket
    {
        /// <summary>
        /// Called when the user needs to sync their money
        /// appears to only be used in the shop
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        public SyncMoney(byte[] data, GameSession client) : base(data, client)
        {
        }

        public override string GetType()
        {
            return "SYNC_MONEY";
        }

        protected override void RunImpl()
        {
            var client = GetClient();
            client.SendPacket(new SendMoneySynced(client.User));
        }
    }
}