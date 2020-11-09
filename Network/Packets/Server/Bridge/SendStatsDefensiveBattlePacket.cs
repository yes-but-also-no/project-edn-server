using Data.Model;
using Sylver.Network.Data;

namespace Network.Packets.Server.Bridge
{
    public class SendStatsDefensiveBattlePacket : IPacketSerializer
    {
        /// <summary>
        /// The user this packet is about
        /// </summary>
        public ExteelUser User { get; set; }
        
        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.SendDefensiveBattleInfo;

        public void Serialize(GamePacket packet)
        {
            // 8 Ints - Unknown if not specified
            packet.Write(100); // 
            packet.Write(100); // 
            packet.Write(100); // Wins
            packet.Write(100); // Desertions
            packet.Write(100); // Losses
            packet.Write(100); // Points
            packet.Write(100); // NPC Kills
            packet.Write(100); // Time
            
            
            packet.Write(1); // Array size
            
            packet.Write(0); // Map ID
            packet.Write(100); // Total record (points?)
            packet.Write(100); // Total ranking (leaderboard?)
            packet.Write(100); // Weekly record
            packet.Write(100); // Weekly ranking
            packet.Write(100); // Last weeks record
            packet.Write(100); // Last weeks ranking
            packet.Write(100); // Last weeks credits earned
        }

    }
}