using Data.Configuration;
using Data.Configuration.Poo;
using Data.Model;
using Sylver.Network.Data;

namespace Network.Packets.Server.Lobby
{
    public class GameCreatedPacket : IPacketSerializer
    {

        /// <summary>
        /// The user this packet is related to
        /// </summary>
        public RoomRecord RoomRecord { get; set; }
        
        /// <summary>
        /// The rooms template info
        /// </summary>
        public GameStats GameStats { get; set; }
        
        /// <summary>
        /// The rooms master
        /// </summary>
        public ExteelUser Master { get; set; }

        /// <summary>
        /// The result code of this packet
        /// </summary>
        public GameCreatedResultCode ResultCode { get; set; }

        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.GameCreated;

        public void Serialize(GamePacket packet)
        {
            // Result code first
            packet.Write((int)ResultCode);

            // Success codes
            if (ResultCode == GameCreatedResultCode.Success)
            {
                packet.Write(RoomRecord.Id); // Id
                
                packet.WriteRoomInfo(RoomRecord, GameStats, Master, 0, (int) GameStatus.Waiting); // Boilerplate
                
                packet.WriteGameString(ServerConfig.Configuration.Global.GameHost); // Host?
            }
        }

    }
    
    /// <summary>
    /// Result codes
    /// There is more, I just have not reversed them all
    /// </summary>
    public enum GameCreatedResultCode : int
    {
        Success = 0,
        Failed = 1
    }
    
    /// <summary>
    /// Game status code
    /// There is more, i just havent reversed them all
    /// </summary>
    public enum GameStatus : int
    {
        Waiting = 0,
        InPlay = 1
    }
}