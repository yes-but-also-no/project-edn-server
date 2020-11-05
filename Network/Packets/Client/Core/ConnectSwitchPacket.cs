using Sylver.Network.Data;

namespace Network.Packets.Client.Core
{
    public class ConnectSwitchPacket : IPacketDeserializer
    {

        /// <summary>
        /// The job code for the server switch
        /// </summary>
        public int JobCode { get; private set; }

        public void Deserialize(GamePacket packet)
        {
            JobCode = packet.Read<int>();
        }

    }
}