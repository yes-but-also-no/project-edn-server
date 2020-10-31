using GameServer.ServerPackets.Chat;

namespace GameServer.ClientPackets.Chat
{
    /// <summary>
    /// Sent when a user leaves a chat channel
    /// </summary>
    public class MsgLeaveChannel : ClientBasePacket
    {
        public MsgLeaveChannel(byte[] data, GameSession client) : base(data, client)
        {
        }

        public override string GetType()
        {
            return "MSG_LEAVE_CHANNEL";
        }

        protected override void RunImpl()
        {
            // Leave channel
            GetClient().Channel.LeaveChannel(GetClient());
            
            // Send result
            GetClient().SendPacket(new MsgLeaveChannelResult());
        }
    }
}