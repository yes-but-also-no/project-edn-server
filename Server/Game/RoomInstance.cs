using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Data.Configuration;
using Data.Configuration.Poo;
using Data.Model;
using GameServer.Game.GameModes;
using GameServer.Managers;
using GameServer.ServerPackets;
using GameServer.ServerPackets.Lobby;
using GameServer.ServerPackets.Room;
using Swan.Logging;

namespace GameServer.Game
{
    /// <summary>
    /// A room is a wrapper for a game instance
    /// Once the game play beings, the room will create a game instance of the correct type
    /// </summary>
    public class RoomInstance
    {
        /// <summary>
        /// The game instance for this room
        /// </summary>
        private GameInstance _gameInstance;
        
        #region ROOM PROPERTIES
        
        /// <summary>
        /// The unique Id of this room/game
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The public name of this game
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The password for this room, if applicable
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// Is this game visible in rooms list?
        /// Only looks like tutorials are hidden for now
        /// </summary>
        public bool Visible { get; set; }
        
        /// <summary>
        /// Does this game have balance set?
        /// </summary>
        public bool Balance { get; set; }
        
        /// <summary>
        /// Does this game have five minute set? (Only for TC)
        /// </summary>
        public bool FiveMinute { get; set; }
        
        /// <summary>
        /// Does this game have SD enabled? (I think its big head mode)
        /// </summary>
        public bool SdMode { get; set; }
        
        /// <summary>
        /// The difficulty of this room (only used in last stand)
        /// </summary>
        public int Difficulty { get; set; }
        
        /// <summary>
        /// The minimum level to join
        /// </summary>
        public int MinLevel { get; set; }
        
        /// <summary>
        /// The max level to join
        /// </summary>
        public int MaxLevel { get; set; }
        
        /// <summary>
        /// The master of this room
        /// </summary>
        public ExteelUser Master => Users.FirstOrDefault(u => u.Id == MasterId);
        
        /// <summary>
        /// The Id of the master of this room
        /// </summary>
        public int MasterId { get; set; }
        
        /// <summary>
        /// Dictionary of all sessions in this room
        /// </summary>
        private readonly ConcurrentDictionary<Guid, GameSession> _sessions = new ConcurrentDictionary<Guid, GameSession>();

        /// <summary>
        /// All sessions in this game
        /// </summary>
        public IEnumerable<GameSession> Sessions => _sessions.Values;
        
        /// <summary>
        /// The users in this room
        /// </summary>
        public IEnumerable<ExteelUser> Users => _sessions.Values.Select(s => s.User);

        /// <summary>
        /// The status of this game
        /// </summary>
        public GameStatus GameStatus { get; set; } = GameStatus.Waiting;
        
        /// <summary>
        /// The game template id for this game
        /// </summary>
        public uint GameTemplate { get; set; }

        /// <summary>
        /// The max number of users who can be in this room
        /// </summary>
        public int Capacity => _gameInstance.GameStats.MaxPlayers;

        /// <summary>
        /// Type of game this room is
        /// </summary>
        public GameType GameType => _gameInstance.GameStats.GameType;

        /// <summary>
        /// Public accessor
        /// </summary>
        public GameInstance GameInstance => _gameInstance;
        
        #endregion
        
        #region MESSAGING
        
        /// <summary>
        /// Multicasts a packet to all users in this room
        /// </summary>
        /// <param name="packet"></param>
        public void MulticastPacket(ServerBasePacket packet)
        {
            var data = packet.Write();
            // Multicast data to all sessions
            foreach (var session in _sessions.Values)
            {
                if (packet.HighPriority)
                    session.Send(data);
                else
                    session.SendAsync(data);
            }
            
            if (ServerConfig.Configuration.Global.LogAllPackets)
                $"[S] 0x{packet.GetId():x2} {packet.GetType()} >>> ALL".Info();

            // TODO: Add config here - if debug
            // Temp disable to lessen spam
            //Console.WriteLine("[S] 0x{0:x2} {1} >>> room: {2}", Color.Green, packet.GetId(), packet.GetType(), Id);
        }
        
        /// <summary>
        /// Multicasts a packet to all users in this room except one
        /// </summary>
        /// <param name="packet"></param>
        public void MulticastPacketWithSession(Func<GameSession, ServerBasePacket> packetCreator)
        {
            // Multicast data to all sessions
            foreach (var session in _sessions.Values)
            {
                var packet = packetCreator(session);
                var data = packet.Write();
                
                if (packet.HighPriority)
                    session.Send(data);
                else
                    session.SendAsync(data);
                
                if (ServerConfig.Configuration.Global.LogAllPackets)
                    $"[S] 0x{packet.GetId():x2} {packet.GetType()} >>> {session}".Info();
            }

            // TODO: Add config here - if debug
            // Temp disable to lessen spam
            //Console.WriteLine("[S] 0x{0:x2} {1} >>> room: {2}", Color.Green, packet.GetId(), packet.GetType(), Id);
        }

        /// <summary>
        /// Sends a batch of packets to the client
        /// </summary>
        /// <param name="packets"></param>
        public void MulticastPacket(IEnumerable<ServerBasePacket> packets, bool highPriority = false)
        {
            IEnumerable<byte> data = new byte[]{};

            foreach (var serverBasePacket in packets)
            {
                data = data.Concat(serverBasePacket.Write());
            }

            var dataCompiled = data.ToArray();
            
            // Multicast data to all sessions
            foreach (var session in _sessions.Values)
            {
                if (highPriority)
                    session.Send(dataCompiled);
                else
                    session.SendAsync(dataCompiled);
            }
        }
        
        #endregion

        #region JOINING / LEAVING

        /// <summary>
        /// Attempts to join this game
        /// </summary>
        /// <param name="session">The session trying to join</param>
        /// <returns></returns>
        public GameEntered TryEnterGame(GameSession session)
        {
            // If game is full
            if (_sessions.Count >= Capacity)
            {
                return new GameEntered(GameEnteredResult.Full);
            }
            
            // TODO: Other checks, too early, banned, no units, etc
            
            // TODO: Joining in progress?
            
            // Success
            
            // Setup team
            session.User.Team = _gameInstance.GetTeamForNewUser();
            
            // Setup score
            session.User.Scores = new PlayerScores();
            
            // Reset ready flag
            session.User.IsReady = false;
            session.IsGameReady = false;
            
            // Announce user to room
            // TODO: Check for game user join packet
            MulticastPacket(new UserEnter(this, session.User));
            
            // Send this users info?
            //MulticastPacket(new UserInfo(this, session.User));
            
            if (_gameInstance.GameState == GameState.WaitingRoom)
                MulticastPacket(new UnitInfo(session.User, session.User.DefaultUnit));
            
            
            // Call hook for game enter
            _gameInstance.OnGameEnter(session);
            
            // Add to session list
            _sessions.TryAdd(session.Id, session);
            
            // Assign to the client
            session.RoomInstance = this;
            
            // Log
            $"[{session}] has joined".Info(source: ToString());
            
            return new GameEntered(GameEnteredResult.Success, this);
        }
        
        /// <summary>
        /// Removes a user from the game
        /// </summary>
        /// <param name="session"></param>
        public void TryLeaveGame(GameSession session)
        {   
            // TEST - Try here?
            MulticastPacket(new UserExit(session.User));
            
            // Remove from session list
            RemoveSession(session.User.SessionId);
            
            // Destroy if empty
            if (!Sessions.Any())
            {
                _gameInstance.OnGameDestroy();
                RoomManager.DeleteRoom(Id);
            }
            // Reassign master
            else if (session.UserId == MasterId)
                MasterId = Sessions.First().UserId;
            
            // TODO: Broadcast update
            //MulticastPacket(new UserExit(this, session.User));
            
            // Clear data
//            session.User.SessionId = Guid.Empty;
            session.RoomInstance = null;
            
            // Call hook for game leave
            _gameInstance.OnGameLeave(session);

            // Log
            $"[{session}] has quit".Info(source: ToString());
            
            // TODO: Destroy this room if the last user has left
        }
        
        /// <summary>
        /// Removes a session from this room
        /// </summary>
        /// <param name="id"></param>
        private void RemoveSession(Guid id)
        {
            // Unregister session by Id
            _sessions.TryRemove(id, out GameSession temp);
        }

        #endregion
        
        #region EVENTS

        /// <summary>
        /// Sets a user's ready state
        /// </summary>
        /// <param name="session"></param>
        public void SetUserReady(GameSession session, bool isReady)
        {
            session.User.IsReady = isReady;
            
            MulticastPacket(new UserInfo(this, session.User));
        }

        /// <summary>
        /// Called when the master trys to start the game
        /// </summary>
        public void TryStartGame()
        {
            var result = 0;

            // Check for all ready
            if (Users.Any(u => !u.IsReady && u != Master))
                result = 1;
            
            // TODO: Any other checks
            
            // If random, pick a map
            if (_gameInstance.GameStats.MapFileName == "RANDOM")
            {
                // Get the first three digits of the template id
                var templateGroup = _gameInstance.GameStats.TemplateGroup;
                
                // Grab all matching
                var eligibleGames = PooReader.Game
                    .Where(g => g.TemplateGroup == templateGroup && g.MapFileName != "RANDOM")
                    .ToList();

                // Get random
                var rnd = new Random();

                var r = rnd.Next(eligibleGames.Count);
                
                // Set template id
                GameTemplate = eligibleGames[r].TemplateId;
                
                // Update
                SettingsChanged();
                
                // Log
                $"Random game found: {GameTemplate}".Debug(source: ToString());
            }
            
            // Notify
            MulticastPacket(new GameStart(this, result));

            // Set state
            GameStatus = GameStatus.InBattle;
            
            // Call hook
            _gameInstance.OnGameLoad();
        }

        /// <summary>
        /// Re-evaluates this room if settings have changed
        /// WIP
        /// </summary>
        public void SettingsChanged()
        {
            // Destroy old room?
            _gameInstance = null;
            
            // Find stats
            var stats = PooReader.Game.First(g => g.TemplateId == GameTemplate);

            // Create new
            switch (stats.GameType)
            {
                case GameType.survival:
                    _gameInstance = new Deathmatch(this, stats);
                    break;
                
                default:
                    throw new NotImplementedException($"Game type: {stats.GameType} is not implemented yet!");
            }
        }
        
        #endregion
        
        public override string ToString()
        {
            return $"[ROOM]<{Id}:{Name}>";
        }
    }
}