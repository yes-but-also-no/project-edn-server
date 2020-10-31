using GameServer.ServerPackets.Chat;

namespace GameServer.ClientPackets.Chat
{
    /// <summary>
    /// Sent when the user wants the messenger channel list
    /// </summary>
    public class MsgGetChannelList : ClientBasePacket
    {
        public MsgGetChannelList(byte[] data, GameSession client) : base(data, client)
        {
        }

        public override string GetType()
        {
            return "MSG_GET_CHANNEL_LIST";
        }

        protected override void RunImpl()
        {
            GetClient().SendPacket(new MsgChannelList());
        }
    }
}