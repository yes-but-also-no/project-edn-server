using GameServer.ServerPackets.Inventory;

namespace GameServer.ClientPackets.Inventory
{
    /// <summary>
    /// Sent when the client requests their current operators or... all operators?
    /// </summary>
    public class RequestOperator : ClientBasePacket
    {
        public RequestOperator(byte[] data, GameSession client) : base(data, client)
        {
        }

        public override string GetType()
        {
            return "INV_REQ_OPERATOR";
        }

        protected override void RunImpl()
        {
            var client = GetClient();
            client.SendPacket(new SendOperatorList(client.User));
        }
    }
}