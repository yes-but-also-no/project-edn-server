using Data;
using Data.Configuration.Poo;
using Data.Model;
using Sylver.Network.Data;

namespace Network.Packets.Server.Lobby
{
    public class GameEnteredPacket : IPacketSerializer
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
        /// Current number of players in the room
        /// </summary>
        public int PlayerCount { get; set; }
        
        /// <summary>
        /// The status of this game
        /// </summary>
        public GameStatus GameStatus { get; set; }
        
        /// <summary>
        /// Result code for this packet
        /// </summary>
        public GameEnteredResult ResultCode { get; set; }
        
        /// <summary>
        /// Only used on the expelled result code
        /// </summary>
        public int BanMinutes { get; set; }
        
        /// <summary>
        /// This packets Id
        /// </summary>
        public ServerPacketType PacketType => ServerPacketType.GameEntered;

        public void Serialize(GamePacket packet)
        {
            // 0 = success
            // 1 = no longer available or technical problem
            // 2 = battle full
            // 3 = battle has started but not in play yet
            // 4 = time limit has expired - maybe game over?
            // 5 = Incorrect login path? Maybe old system where people joined from web?
            // 6 = No units ready due to repair issues
            // 7 = Cannot participate?
            // 8 = Something about lower class? Maybe an old balance system?
            // 9 = No rooms that match criteria? maybe quick join result?
            // 0xa = Expelled - cannot enter for X minutes
            packet.Write((int)ResultCode);
            
            packet.Write((int)GameStatus); // Status code

            switch (ResultCode)
            {
                case GameEnteredResult.Expelled:
                    packet.Write(BanMinutes);
                    break;
                
                case GameEnteredResult.Success:
                    packet.Write(0); // Unknown
                    packet.WriteRoomInfo(RoomRecord, GameStats, Master, PlayerCount, GameStatus);
                    break;
                
                // TODO: Handle result codes if needed
                default:
                    packet.Write(0);
                    break;
            }
        }

    }
    
    /// <summary>
    /// Results for trying to join a game
    /// </summary>
    public enum GameEnteredResult
    {
        Success = 0,
        NotAvailable = 1,
        Full = 2,
        JustStarted = 3,
        JustFinished = 4,
        BadLoginPath = 5,
        NoUnitsAvailable = 6,
        CannotParticipate = 7,
        LowerClass = 8,
        NoRoomsMatch = 9,
        Expelled = 0xa
    }
}