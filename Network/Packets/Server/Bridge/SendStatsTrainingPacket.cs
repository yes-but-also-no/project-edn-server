using Data.Model;
using Sylver.Network.Data;

namespace Network.Packets.Server.Bridge
{
    public class SendStatsTrainingPacket : IPacketSerializer
    {
        /// <summary>
        /// The user this packet is about
        /// </summary>
        public ExteelUser User { get; set; }
        
        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.SendTrainingInfo;

        public void Serialize(GamePacket packet)
        {
            // 13 Ints - Unknown if not specified
            packet.Write(100);
            packet.Write(100);
            packet.Write(100); 
            packet.Write(100); 
            packet.Write(100);
            packet.Write(100);
            packet.Write(100);
            packet.Write(100);
            packet.Write(100);
            packet.Write(100);
            packet.Write(100);
            packet.Write(100);
            packet.Write(100);
        }

    }
}