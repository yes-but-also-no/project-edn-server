using Data.Model;
using Sylver.Network.Data;

namespace Network.Packets.Server.Bridge
{
    public class SendStatsTeamBattlePacket : IPacketSerializer
    {
        /// <summary>
        /// The user this packet is about
        /// </summary>
        public ExteelUser User { get; set; }
        
        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.SendTeamBattleInfo;

        public void Serialize(GamePacket packet)
        {
            // 15 Ints - Unknown if not specified
            packet.Write(100); 
            packet.Write(100);
            packet.Write(100); // Kills 
            packet.Write(100); // Assists
            packet.Write(100); // Aerogate (captures?)
            packet.Write(100); 
            packet.Write(100); // Deaths
            packet.Write(100); // Wins
            packet.Write(100); // Losses
            packet.Write(100); // Draws
            packet.Write(100); // Desertions
            packet.Write(100); // Points
            packet.Write(100); // High score
            packet.Write(100); // Time
            packet.Write(100); // 
        }

    }
}