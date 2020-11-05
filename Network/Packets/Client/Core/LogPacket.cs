using Sylver.Network.Data;

namespace Network.Packets.Client.Core
{
    public class LogPacket  : IPacketDeserializer
    {
        /// <summary>
        /// Gets the logged message
        /// </summary>
        public string LogMessage { get; private set; }
        
        public void Deserialize(GamePacket packet)
        {
            LogMessage = packet.ReadGameString();
        }
    }
}