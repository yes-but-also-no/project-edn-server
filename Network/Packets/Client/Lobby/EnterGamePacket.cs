using Sylver.Network.Data;

namespace Network.Packets.Client.Lobby
{
    public class EnterGamePacket : IPacketDeserializer
    {
        /// <summary>
        /// The room this user wants to enter
        /// </summary>
        public int RoomId { get; private set; }

        public void Deserialize(GamePacket packet)
        {
            RoomId = packet.Read<int>();
        }

    }
}