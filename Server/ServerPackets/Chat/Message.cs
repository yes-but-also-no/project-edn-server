namespace GameServer.ServerPackets.Chat
{
    /// <summary>
    /// Sends a chat message
    /// </summary>
    public class Message : ServerBasePacket
    {
        /// <summary>
        /// The message to send
        /// </summary>
        private readonly string _message;
        
        /// <summary>
        /// The name of the message sender
        /// </summary>
        private readonly string _name;

        private readonly int _a;
        private readonly int _b;

        public Message(string name, string message, int a = 0, int b = 0)
        {
            _name = name;
            _message = message;

            _a = a;
            _b = b;
        }
        
        public override string GetType()
        {
            return "MESSAGE";
        }

        public override byte GetId()
        {
            return 0x05;
        }

        protected override void WriteImpl()
        {
            WriteInt(_a); // Unknown
            WriteInt(_b); // Unknown
            
            WriteString(_name);
            WriteString(_message);
        }
    }
}