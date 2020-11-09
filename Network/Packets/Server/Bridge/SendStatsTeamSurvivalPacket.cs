using Data.Model;
using Sylver.Network.Data;

namespace Network.Packets.Server.Bridge
{
    public class SendStatsTeamSurvivalPacket : IPacketSerializer
    {
        /// <summary>
        /// The user this packet is about
        /// </summary>
        public ExteelUser User { get; set; }
        
        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.SendTeamSurvivalInfo;

        public void Serialize(GamePacket packet)
        {
            // 13 Ints - Unknown if not specified
            packet.Write(100); 
            packet.Write(100);// 
            packet.Write(100); // Kills
            packet.Write(100); // Assists
            packet.Write(100); // 
            packet.Write(100); // Deaths
            packet.Write(100); // Wins
            packet.Write(100); // Losses
            packet.Write(100); // Draws
            packet.Write(100); // Desertions
            packet.Write(100); // Points
            packet.Write(100); // High Score
            packet.Write(100); // Time
        }

    }
}