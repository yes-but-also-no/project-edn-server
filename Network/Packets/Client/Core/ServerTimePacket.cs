using Sylver.Network.Data;

namespace Network.Packets.Client.Core
{
    public class ServerTimePacket : IPacketDeserializer
    {

        /// <summary>
        /// The clients current running time, in milliseconds
        /// </summary>
        public int ClientTime { get; private set; }

        public void Deserialize(GamePacket packet)
        {
            ClientTime = packet.Read<int>();
        }

    }
}