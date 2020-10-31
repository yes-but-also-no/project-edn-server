using GameServer.ServerPackets.Chat;

namespace GameServer.ClientPackets.Chat
{
    /// <summary>
    /// Sent when the user wishes to connect to chat
    /// </summary>
    public class MsgConnect : ClientBasePacket
    {
        public MsgConnect(byte[] data, GameSession client) : base(data, client)
        {
        }

        public override string GetType()
        {
            return "MSG_CONNECT";
        }

        protected override void RunImpl()
        {
            // Always return true for now?
            GetClient().SendPacket(new MsgConnectResult());
        }
    }
}