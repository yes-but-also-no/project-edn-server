using System;

namespace Engine
{
    /// <summary>
    /// This is the master game engine. It is instantiated for each game and handles all
    /// game engine logic
    /// </summary>
    public class GameEngine
    {
        #region PUBLIC
        
        /// <summary>
        /// The tick rate of the server
        /// This is usually the game fps, but can be set higher if needed
        /// Only touch if you know what you are doing
        /// This affects the maximum speed at which Entity::Tick() can be called
        /// </summary>
        public int TickRate { get; set; }

        /// <summary>
        /// This is the time that the engine has been running for
        /// Most client packets will be checked against this
        /// TODO: If this causes performance issues, we can use an instance variable and update it on each tick
        /// </summary>
        public int EngineTime => (int) (DateTime.UtcNow - _engineStartTime).TotalMilliseconds;
        
        #endregion
        
        #region PRIVATE

        /// <summary>
        /// When the engine was started
        /// </summary>
        private readonly DateTime _engineStartTime;
        
        #endregion

        /// <summary>
        /// Default constructor shows start time as now
        /// </summary>
        public GameEngine()
        {
            _engineStartTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Constructor for set start time
        /// </summary>
        /// <param name="engineStartTime">When the engine has been started</param>
        public GameEngine(DateTime engineStartTime)
        {
            _engineStartTime = engineStartTime;
        }
    }
}