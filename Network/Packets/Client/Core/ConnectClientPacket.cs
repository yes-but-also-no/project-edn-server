using Sylver.Network.Data;

namespace Network.Packets.Client.Core
{
    public class ConnectClientPacket : IPacketDeserializer
    {

        /// <summary>
        /// The login username
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// The login password
        /// </summary>
        public string Password { get; private set; }

        public void Deserialize(GamePacket packet)
        {
            UserName = packet.ReadGameString();
            Password = packet.ReadGameString();
        }

    }
}