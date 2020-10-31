namespace GameServer.ServerPackets.Chat
{
    /// <summary>
    /// Confirms the user has left a channel
    /// </summary>
    public class MsgLeaveChannelResult : ServerBasePacket
    {
        public override string GetType()
        {
            return "MSG_LEAVE_CHANNEL_RESULT";
        }

        public override byte GetId()
        {
            return 0xdc;
        }

        protected override void WriteImpl()
        {
            WriteInt(0); // Result code
        }
    }
}