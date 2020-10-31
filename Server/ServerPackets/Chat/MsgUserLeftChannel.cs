namespace GameServer.ServerPackets.Chat
{
    /// <summary>
    /// Sent when a user leaves the channel
    /// </summary>
    public class MsgUserLeftChannel : ServerBasePacket
    {
        /// <summary>
        /// The channel
        /// </summary>
        private readonly int _channelId;

        /// <summary>
        /// The user
        /// </summary>
        private readonly int _userId;

        public MsgUserLeftChannel(int userId, int channelId)
        {
            _userId = userId;
            _channelId = channelId;
        }
        
        public override string GetType()
        {
            return "MSG_USER_LEFT_CHANNEL";
        }

        public override byte GetId()
        {
            return 0xdd;
        }

        protected override void WriteImpl()
        {
            WriteInt(_channelId);
            WriteInt(_userId);
        }
    }
}