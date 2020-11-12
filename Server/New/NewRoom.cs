using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Data.Configuration;
using Data.Configuration.Poo;
using Data.Model;
using Game;

namespace GameServer.New
{
    /// <summary>
    /// The room is the entry point into a game
    /// It tracks a game object and handles loading of game modes, settings,
    /// tracking players, etc
    /// </summary>
    public class NewRoom : IDisposable
    {
        /// <summary>
        /// The game instance for this room
        /// </summary>
        public global::Game.Game Game { get; private set; }
        
        /// <summary>
        /// The room record for this room
        /// </summary>
        public RoomRecord RoomRecord { get; private set; }
        
        /// <summary>
        /// The game template stats for this game
        /// </summary>
        public GameStats GameStats { get; private set; }
        
        /// <summary>
        /// Storage of all players in this game
        /// </summary>
        private readonly Dictionary<int, GameClient> _clients = new Dictionary<int, GameClient>();
        
        #region ROOM PROPERTIES
        
        /// <summary>
        /// The master of this room
        /// </summary>
        public ExteelUser Master => Users.FirstOrDefault(u => u.Id == MasterId);
        
        /// <summary>
        /// The master of this room
        /// </summary>
        public int MasterId { get; private set; }

        /// <summary>
        /// All sessions in this game
        /// </summary>
        public IEnumerable<GameClient> Clients => _clients.Values;
        
        /// <summary>
        /// The users in this room
        /// </summary>
        public IEnumerable<ExteelUser> Users => _clients.Values.Select(s => s.User);

        /// <summary>
        /// The status of this game
        /// </summary>
        public GameState GameStatus { get; set; } = GameState.WaitingRoom;

        /// <summary>
        /// The max number of users who can be in this room
        /// </summary>
        public int Capacity => GameStats.MaxPlayers;

        /// <summary>
        /// Type of game this room is
        /// </summary>
        public GameType GameType => GameStats.GameType;

        #endregion
        
        #region PUBLIC METHODS

        /// <summary>
        /// Sets this room record
        /// TODO: Broadcast settings change to clients
        /// </summary>
        /// <param name="record"></param>
        public void SetRoomRecord(RoomRecord record)
        {
            // Set
            RoomRecord = record;
            
            // Get stats
            GameStats = PooReader.Game.First(g => g.TemplateId == RoomRecord.TemplateId);
        }

        /// <summary>
        /// Sets the master to a specified user
        /// TODO: Broadcast change to clients
        /// </summary>
        /// <param name="UserId"></param>
        public void SetMaster(int masterId)
        {
            MasterId = masterId;
        }
        
        #endregion
        
        public void Dispose()
        {
            Game?.Dispose();
        }
        
        public override string ToString()
        {
            return $"[ROOM]<{RoomRecord.Id}:{RoomRecord.Name}>";
        }
    }
    
    
}