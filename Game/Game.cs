using System.Reflection;
using Engine;
using MoonSharp.Interpreter;

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
        private readonly Script _mainScript;

        public Game()
        {
            // Setup scripting
            _mainScript = new Script {Options = {ScriptLoader = new GameScriptLoader()}};
            _mainScript.DoFile("init.lua");
            
            // Register our selves with the engine
            _engine.RegisterAssembly(Assembly.GetExecutingAssembly());
            
            // TEMP: Start the engine
            _engine.Start();
            
            // TEMP: Create an entity
            _engine.CreateEntity("TestEntity", "my_test_entity");
        }
    }
}