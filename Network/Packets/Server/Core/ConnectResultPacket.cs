using Data.Configuration;
using Data.Model;
using Sylver.Network.Data;

namespace Network.Packets.Server.Core
{
    public class ConnectResultPacket : IPacketSerializer
    {

        /// <summary>
        /// The user this packet is related to
        /// </summary>
        public ExteelUser User { get; set; }
        
        /// <summary>
        /// The result code of this packet
        /// </summary>
        public ConnectResultCode ResultCode { get; set; }
        
        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.ConnectResult;

        public void Serialize(GamePacket packet)
        {
            // Result code first
            packet.Write((int)ResultCode);

            // Success codes
            if (ResultCode == ConnectResultCode.Success || ResultCode == ConnectResultCode.SuccessNewAccount)
            {
                packet.Write(User.Id); // User Id
                packet.WriteGameString(""); // Unknown - maybe clan name?
                packet.WriteGameString(User.Username); // User name
                packet.WriteGameString(User.Nickname); // Nick name
                packet.Write(User.Level); // Level
                
                for (var i = 0; i < 256; i++)
                {
                    packet.Write((byte)0); // Config data??
                }
                packet.WriteGameString(ServerConfig.Configuration.Global.GameHost); // Unsure - I think this is the game host
                packet.Write(255); // Unknown
            }
        }
        
    }

    /// <summary>
    /// Result codes
    /// There is more, I just have not reversed them all
    /// </summary>
    public enum ConnectResultCode : int
    {
        Success = 0,
        SuccessNewAccount = -6,
        UserNotFound = 1,
        PasswordWrong = 2,
    }
}