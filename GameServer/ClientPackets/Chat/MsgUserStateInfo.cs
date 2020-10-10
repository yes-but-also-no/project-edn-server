namespace GameServer.ClientPackets.Chat
{
    /// <summary>
    /// The client is sending their chat state info
    /// </summary>
    public class MsgUserStateInfo : ClientBasePacket
    {
        public MsgUserStateInfo(byte[] data, GameSession client) : base(data, client)
        {
        }

        public override string GetType()
        {
            return "MSG_USER_STATE_INFO";
        }

        protected override void RunImpl()
        {
            GetClient().SendPacket(new ServerPackets.Chat.MsgUserStateInfo(GetClient().User));
        }
    }
}