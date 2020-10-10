using GameServer.Managers;

namespace GameServer.ClientPackets.Chat
{
    /// <summary>
    /// Sent when the client sends a message
    /// </summary>
    public class MsgChannelChatting : ClientBasePacket
    {
        /// <summary>
        /// The channel it was sent on
        /// </summary>
        private readonly int _channel;
        
        /// <summary>
        /// The message they sent
        /// </summary>
        private readonly string _message;
        
        public MsgChannelChatting(byte[] data, GameSession client) : base(data, client)
        {           
            _channel = GetInt();

            _message = GetString();
        }

        public override string GetType()
        {
            return "MSG_CHANNEL_CHATTING";
        }

        protected override void RunImpl()
        {
            // Send message
            ChatManager.Channels[_channel].SendMessage(GetClient(), _message);
        }
    }
}