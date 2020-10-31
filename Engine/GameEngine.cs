using System;
using Cysharp.Threading;
using Swan.Logging;

namespace Engine
{
    /// <summary>
    /// This is the master game engine. It is instantiated for each game and handles all
    /// game engine logic
    /// </summary>
    public class GameEngine
    {
        #region PROPERTIES
        
        /// <summary>
        /// The unique id of this engine instance
        /// </summary>
        public readonly Guid Id = Guid.NewGuid();
        
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
        
        #region PRIVATE FIELDS

        /// <summary>
        /// When the engine was started
        /// </summary>
        private readonly DateTime _engineStartTime;
        
        /// <summary>
        /// Logic looper instance for this game engine
        /// </summary>
        private LogicLooperPool _looper;

        #endregion

        #region PUBLIC METHODS
        
        /// <summary>
        /// Default constructor shows start time as now
        /// </summary>
        public GameEngine() : this(DateTime.UtcNow) {}

        /// <summary>
        /// Constructor for set start time
        /// </summary>
        /// <param name="engineStartTime">When the engine has been started</param>
        public GameEngine(DateTime engineStartTime)
        {
            // Set start time
            _engineStartTime = engineStartTime;

            // Log
            $"Engine instance <{Id}> Created!".Info("[GameEngine]");
        }

        /// <summary>
        /// Starts the game engine
        /// </summary>
        public async void Start()
        {
            // Create looper
            _looper = new LogicLooperPool(
                TickRate,
                Environment.ProcessorCount,
                RoundRobinLogicLooperPoolBalancer.Instance
            );
            
            // Start game loop
            await _looper.RegisterActionAsync(Tick);
            
            // Log
            $"Started".Info(ToString());
        }

        /// <summary>
        /// Shuts down the game engine
        /// </summary>
        public async void Stop()
        {
            // Stop loop
            await _looper.ShutdownAsync(TimeSpan.Zero);
            
            // TODO: Do we need to call this?
            _looper.Dispose();
            
            // Log
            $"Stopped".Info(ToString());
        }
        
        #endregion
        
        #region PRIVATE METHODS

        /// <summary>
        /// Main engine tick
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        private bool Tick(in LogicLooperActionContext ctx)
        {
            // DO TICK
            
            // Log
            $"Ticked".Info(ToString());
            
            // Return true to keep going
            return true;
        }
        
        #endregion

        /// <summary>
        /// To string override
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[GameEngine]<{Id}>";
        }
    }
}