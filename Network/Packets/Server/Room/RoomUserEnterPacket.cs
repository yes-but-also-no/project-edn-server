using Data.Model;
using Sylver.Network.Data;

namespace Network.Packets.Server.Room
{
    public class RoomUserEnterPacket : IPacketSerializer
    {
        /// <summary>
        /// The user who joined
        /// </summary>
        public ExteelUser User { get; set; }

        /// <summary>
        /// Are they master
        /// </summary>
        public bool IsMaster { get; set; }
        
        /// <summary>
        /// Are they ready
        /// </summary>
        public bool IsReady { get; set; }

        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.RoomUserEnter;

        public void Serialize(GamePacket packet)
        {
            packet.WriteRoomUserInfo(User, IsMaster, IsReady);
        }

    }
}