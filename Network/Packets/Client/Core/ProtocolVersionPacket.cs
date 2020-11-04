using Sylver.Network.Data;

namespace Network.Packets.Client.Core
{
    public class ProtocolVersionPacket : IPacketDeserializer
    {

        /// <summary>
        /// The protocol version this client is running
        /// </summary>
        public int Version { get; private set; }

        public void Deserialize(INetPacketStream packet)
        {
            Version = packet.Read<int>();
        }

    }
}