using Data.Model;
using Sylver.Network.Data;

namespace Network.Packets.Server.Bridge
{
    public class SendBestInfoPacket : IPacketSerializer
    {
        /// <summary>
        /// The user this packet is about
        /// </summary>
        public ExteelUser User { get; set; }

        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.SendBestInfo;

        public void Serialize(GamePacket packet)
        {
            // Overall has a mysterious string and int header
            packet.Write(0);
            packet.WriteGameString("OverallInfo");
            
            // 13 Ints - Unknown if not specified
            packet.Write(100);
            packet.Write(100);
            packet.Write(100); 
            packet.Write(100); 
            packet.Write(100); // Kills
            packet.Write(100); // Deaths
            packet.Write(100); // Assist
            packet.Write(100);
            packet.Write(100); // Wins
            packet.Write(100); // Lose
            packet.Write(100); // Points
            packet.Write(100);// Best
            packet.Write(100);
        }

    }
}