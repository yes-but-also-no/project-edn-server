using Data.Model;

namespace GameServer.ServerPackets.Chat
{
    /// <summary>
    /// Sent when a user sends a chat message
    /// </summary>
    public class MsgChannelChatting : ServerBasePacket
    {
        /// <summary>
        /// User who sent the message
        /// </summary>
        private readonly int _userId;

        /// <summary>
        /// Channel the message was sent on
        /// </summary>
        private readonly int _channel;

        /// <summary>
        /// The message
        /// </summary>
        private readonly string _message;

        public MsgChannelChatting(ExteelUser user, int channel, string message)
        {
            _userId = user.Id;
            _channel = channel;
            _message = message;
        }

        public MsgChannelChatting(int userId, int channel, string message)
        {
            _userId = userId;
            _channel = channel;
            _message = message;
        }
        
        public override string GetType()
        {
            return "MSG_CHANNEL_CHATTING";
        }

        public override byte GetId()
        {
            return 0xde;
        }

        protected override void WriteImpl()
        {
            WriteInt(_channel);
            WriteInt(_userId);
            WriteString(_message);
        }
    }
}