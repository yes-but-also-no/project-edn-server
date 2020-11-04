using Sylver.Network.Data;

namespace Network.Packets
{
    public class LogPacket  : IPacketDeserializer
    {
        /// <summary>
        /// Gets the logged message
        /// </summary>
        public string LogMessage { get; private set; }
        
        public void Deserialize(INetPacketStream packet)
        {
            LogMessage = packet.Read<string>();
        }
    }
}