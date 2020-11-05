using Data.Model;
using Sylver.Network.Data;

namespace Network.Packets.Server.Bridge
{
    public class SendAvatarInfoPacket : IPacketSerializer
    {
        /// <summary>
        /// The user this packet is about
        /// </summary>
        public ExteelUser User { get; set; }

        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.SendAvatarInfo;

        public void Serialize(GamePacket packet)
        {
            packet.Write(0x00b65355); // Unknown - Id? 55 53 b6 00 in packet
            packet.WriteGameString("ncevent888"); // Unknown - ncevent888 in packet?
            
            packet.WriteGameString(User.Clan?.Name ?? ""); // Clan name
            
            packet.Write(User.Coins); // NC Coins
            packet.WriteGameString(User.Callsign); // Callsign
            
            packet.Write(0); // Unknown
            packet.Write(0); // Unknown
            packet.Write(User.Level); // Level
            packet.Write(0); // Unknown
            packet.Write(User.Rank); // Rank
            
            packet.Write(User.Experience); // Exp
            packet.Write(User.Credits); // Credits
            
            packet.Write(1); // Unknown
            packet.Write(2); // Unknown
            packet.Write(3); // Unknown
            packet.Write(4); // Unknown
            packet.Write(5); // Unknown
            packet.Write(6); // Unknown
            
            packet.Write(0); // Unknown
            packet.Write(User.Clan?.Id ?? 0); // Clan id
            
            packet.Write(9); // Unknown
            packet.Write(10); // Unknown
            packet.Write(11); // Unknown
            packet.Write(12); // Unknown
            packet.Write(13); // Unknown
            
            // Unknown - Int[64] Array
            for (var i = 0; i < 64; i++)
            {
                packet.Write(0);
            }
            
            // Unknown - FArray<Int> 
            packet.Write(0); // Size
            
            // Unknown - Int[6] Array
            for (var i = 0; i < 6; i++)
            {
                packet.Write(0);
            }
            
            // Pilot info
            packet.WritePilotInfo(User.PilotInfo);
            
            // Footer info - See function above for offset issue
            packet.Write(0); // Unknown
            packet.Write(10); // Unknown
            packet.Write(311126400); // Time since last played in milliseconds
            packet.Write(1); // Credits awarded for last stand
        }

    }
}