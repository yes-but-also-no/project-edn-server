using Sylver.Network.Data;

namespace Network.Packets.Client.Core
{
    public class SwitchServer : IPacketDeserializer
    {

        /// <summary>
        /// The desired server to switch to
        /// </summary>
        public string Server { get; private set; }

        public void Deserialize(GamePacket packet)
        {
            Server = packet.ReadGameString();
        }

    }
}