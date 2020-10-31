using GameServer.ServerPackets.Chat;

namespace GameServer.ClientPackets.Chat
{
    /// <summary>
    /// Sent when the client wants their messenger buddy list
    /// </summary>
    public class MsgGetBuddyList : ClientBasePacket
    {
        public MsgGetBuddyList(byte[] data, GameSession client) : base(data, client)
        {
        }

        public override string GetType()
        {
            return "MSG_GET_BUDDY_LIST";
        }

        protected override void RunImpl()
        {
            GetClient().SendPacket(new MsgBuddyList());
        }
    }
}