using GameServer;

namespace Data.Packets
{
    /// <summary>
    /// Base packet for all packets in the server
    /// </summary>
    public abstract class BasePacket
    {
        /// <summary>
        /// The client session this packet is for
        /// </summary>
        private readonly GameSession _client;

        protected BasePacket()
        {
            
        }
        
        protected BasePacket(GameSession client)
        {
            _client = client;
        }

        public GameSession GetClient()
        {
            return _client;
        }
        
        /// <summary>
        /// Gets the packet type of this packet
        /// </summary>
        /// <returns>type of packet</returns>
        public abstract string GetType();
        
        public bool HighPriority = false;
    }
}