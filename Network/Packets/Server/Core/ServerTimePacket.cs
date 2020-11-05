namespace Network.Packets.Server.Core
{
    public class ServerTimePacket : IPacketSerializer
    {
        /// <summary>
        /// The current server time
        /// </summary>
        public int ServerTime { get; set; }

        /// <summary>
        /// The client time for the client requesting this packet
        /// Currently just using an echo of what they sent
        /// </summary>
        public int ClientTime { get; set; }

        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.ServerTime;

        public void Serialize(GamePacket packet)
        {
            packet.Write(ServerTime);
            packet.Write(ClientTime);
        }

    }
}