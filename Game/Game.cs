using System;
using System.IO;
using System.Reflection;
using Core;
using Engine;
using Engine.Entities;
using NLua;

namespace Game
{
    /// <summary>
    /// This is the Exteel specific game instance. This will interface with this engine,
    /// Handle game modes, and interpret lua script
    /// </summary>
    public class Game
    {
        /// <summary>
        /// The game engine reference
        /// </summary>
        private readonly GameEngine _engine = new GameEngine {TickRate = 30};
        
        /// <summary>
        /// The main script reference for this game
        /// </summary>
        private readonly Lua _lua;
        
        /// <summary>
        /// Game specific signal hub
        /// </summary>
        internal SignalHub SignalHub = new SignalHub();

        public Game()
        {
            // Create state
            _lua = new Lua();
            
            LuaEngine.LuaEngine.LoadAllEntities(_lua);

            _lua["createEntity"] = (Func<string, string, Entity>)_engine.CreateEntity;
            
            // TEMP
            _lua["engine"] = _engine;

            // Register our selves with the engine
            _engine.RegisterAssembly(Assembly.GetExecutingAssembly());
            
            // Register our signal hub with the engine
            _engine.RegisterSignalHub(SignalHub);
            
            // TEMP: Start the engine
            _engine.Start();

            _lua.DoFile(Path.Combine(Directory.GetCurrentDirectory(), "Scripts", "init.lua"));

            // TEMP: Create an entity
            //_engine.CreateEntity("TestEntity", "my_test_entity");
        }
    }
}