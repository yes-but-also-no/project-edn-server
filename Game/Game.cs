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
        internal readonly GameEngine Engine = new GameEngine {TickRate = 30};
        
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

            _lua["createEntity"] = (Func<string, string, Entity>)Engine.CreateEntity;
            
            // TEMP
            _lua["engine"] = Engine;

            // Register our selves with the engine
            Engine.RegisterAssembly(Assembly.GetExecutingAssembly());
            
            // Register our signal hub with the engine
            Engine.RegisterSignalHub(SignalHub);
            
            // TEMP: Start the engine
            Engine.Start();

            _lua.DoFile(Path.Combine(Directory.GetCurrentDirectory(), "Scripts", "init.lua"));

            // TEMP: Create an entity
            //_engine.CreateEntity("TestEntity", "my_test_entity");
        }
    }
}