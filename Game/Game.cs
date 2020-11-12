using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Core;
using Data.Configuration;
using Data.Model;
using Engine;
using Engine.Entities;
using NLua;

namespace Game
{
    /// <summary>
    /// This is the Exteel specific game instance. This will interface with this engine,
    /// Handle game modes, and interpret lua script
    /// </summary>
    public class Game : IDisposable
    {
        
        #region PRIVATE
        
        /// <summary>
        /// The game engine reference
        /// </summary>
        internal readonly GameEngine Engine;
        
        /// <summary>
        /// The main script reference for this game
        /// </summary>
        private readonly Lua _lua;
        
        /// <summary>
        /// Game specific signal hub
        /// </summary>
        public readonly SignalHub SignalHub = new SignalHub();
        
        /// <summary>
        /// Dictionary for storing players
        /// </summary>
        internal readonly Dictionary<Guid, Player> Players = new Dictionary<Guid, Player>();
        
        #endregion
        
        #region STATE
        
        /// <summary>
        /// The status of this game
        /// </summary>
        public GameState GameStatus { get; set; } = GameState.WaitingRoom;
        
        #endregion

        public Game()
        {
            // Create engine
            Engine = new GameEngine {TickRate = ServerConfig.Configuration.Global.GameFps};
            
            // Create state
            _lua = new Lua();
            
            LuaEngine.LuaEngine.LoadAllEntities(_lua);

            _lua["createEntity"] = (Func<string, string, Entity>)Engine.CreateEntity;
            
            // TEMP
            _lua["engine"] = Engine;

            // Register our selves with the engine
            Engine.RegisterAssembly(Assembly.GetExecutingAssembly());
            
            // Register our signal hub with the engine
            Engine.RegisterSignalHub(SignalHub);
            
            // TEMP: Start the engine
            //Engine.Start();

            _lua.DoFile(Path.Combine(Directory.GetCurrentDirectory(), "Scripts", "init.lua"));

            // TEMP: Create an entity
            //_engine.CreateEntity("TestEntity", "my_test_entity");
        }

        public void Dispose()
        {
            Engine?.Dispose();
            _lua?.Dispose();
        }

        /// <summary>
        /// Adds a player to this game
        /// </summary>
        /// <returns></returns>
        public Guid AddPlayer(ExteelUser user)
        {
            // Create them
            var ply = new Player(user);
            
            // Add
            Players.Add(ply.Id, ply);

            // Return Id
            return ply.Id;
        }
    }
    
    /// <summary>
    /// Temp enum, until we can see if game status can hold this info
    /// </summary>
    public enum GameState
    {
        WaitingRoom = 0,
        WaitingForPlayers = 1,
        AllPlayersReady = 2,
        WaitingToStart = 3,
        InGame = 4,
        GameOver = 5,
        Destroyed = 6,
    }
}