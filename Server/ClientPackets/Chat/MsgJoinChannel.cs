using GameServer.Managers;

namespace GameServer.ClientPackets.Chat
{
    /// <summary>
    /// Sent when the client wants to join a channel
    /// </summary>
    public class MsgJoinChannel : ClientBasePacket
    {
        /// <summary>
        /// The channel to join
        /// </summary>
        private readonly string _channel;
        
        public MsgJoinChannel(byte[] data, GameSession client) : base(data, client)
        {                
            GetBool(); // Unknown? Maybe two byte id?

            _channel = GetString().Trim('\0');
        }

        public override string GetType()
        {
            return "MSG_JOIN_CHANNEL";
        }

        protected override void RunImpl()
        {
            GetClient().SendPacket(ChatManager.TryJoinChannel(GetClient(), _channel));
        }
    }
}