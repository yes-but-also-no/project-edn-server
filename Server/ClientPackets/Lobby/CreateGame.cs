using GameServer.Game;
using GameServer.Managers;
using GameServer.ServerPackets.Lobby;
using Swan.Logging;

namespace GameServer.ClientPackets.Lobby
{
    /// <summary>
    /// Sent when the client wants to create a game
    /// </summary>
    public class CreateGame : ClientBasePacket
    {
        /// <summary>
        /// The newly created room
        /// </summary>
        private readonly RoomInstance _newRoom;
        
        public CreateGame(byte[] data, GameSession client) : base(data, client)
        {
            //debug = true;

            _newRoom = new RoomInstance
            {
                Name = GetString(),
                Password = GetString().Trim('\0'),
                GameTemplate = GetUInt(),
            };
            
            // Skip unknowns
            GetInt(); // Unknown, always zero so far

            _newRoom.Difficulty = GetInt();
            _newRoom.Balance = GetBool();
            _newRoom.FiveMinute = GetBool();
            _newRoom.SdMode = GetBool();
            _newRoom.Visible = GetBool(); // Zero on tutorial

            _newRoom.MaxLevel = GetInt();
            _newRoom.MinLevel = GetInt();

            _newRoom.MasterId = client.UserId;
            
            
//            GetString().Info("Room Name");
//            GetString().Info("Password");
//            GetInt().ToString().Info("Template Id");
//            GetInt().ToString().Info("Unknown3"); // Always zero
//            GetInt().ToString().Info("Difficulty");
//            GetByte().ToString("X2").Info("Balance");
//            GetByte().ToString("X2").Info("5 Minute");
//            GetByte().ToString("X2").Info("SD Battle");
//            GetByte().ToString("X2").Info("Game Joinable?"); // Always one, except zero on tutorial?
//            GetInt().ToString().Info("Max Level");
//            GetInt().ToString().Info("Min Level");
        }

        public override string GetType()
        {
            return "LOBBY_CREATE_GAME";
        }

        protected override void RunImpl()
        {
            $"User {GetClient().GetUserName()} created room {_newRoom.Name}".Info();
            var success = RoomManager.CreateRoom(_newRoom);
            
            // Send
            GetClient().SendPacket(new GameCreated(success ? _newRoom : null));
        }
    }
}