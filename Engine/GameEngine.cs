using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cysharp.Threading;
using Engine.Entities;
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

        /// <summary>
        /// This holds a reference to all entities in the game engine
        /// </summary>
        private readonly Dictionary<Guid, Entity> _entities = new Dictionary<Guid, Entity>();
        
        /// <summary>
        /// This holds a list of all registered assemblies to search when creating classes
        /// </summary>
        private readonly HashSet<Assembly> _assemblies = new HashSet<Assembly>{Assembly.GetExecutingAssembly()};

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
        
        #region ENTITIES

        /// <summary>
        /// Registers a new assembly to be searched when creating classes
        /// </summary>
        /// <param name="assembly"></param>
        public void RegisterAssembly(Assembly assembly)
        {
            // Add to hash set
            _assemblies.Add(assembly);
        }
        
        /// <summary>
        /// Creates a new entity
        /// </summary>
        /// <param name="engineName"></param>
        /// <param name="engineClass"></param>
        /// <returns></returns>
        public Entity CreateEntity(string engineClass, string engineName)
        {
            // Get constructor
            var type = _assemblies
                .SelectMany(a => a.GetTypes())
                .Where(typeof(Entity).IsAssignableFrom)
                .FirstOrDefault(t => t.Name == engineClass);
            
            // Check for errors
            if (type == null)
            {
                $"Could not find entity with engine class of {engineClass}!".Error(ToString());
                return null;
            }
                
            // Create it
            var ent = Activator.CreateInstance(type, this) as Entity;
            
            // Check for errors
            if (ent == null)
            {
                $"Could not create entity with engine class of {engineClass}!".Error(ToString());
                return null;
            }

            ent.EngineClass = engineClass;
            ent.EngineName = engineName;

            // Add to list
            _entities.Add(ent.EngineId, ent);
            
            // Initialize
            ent.Initialize();
            
            // return
            return ent;
        }

        /// <summary>
        /// Removes an entity by its engine id
        /// </summary>
        /// <param name="engineId"></param>
        public void RemoveEntity(Guid engineId)
        {
            // Get entity
            var ent = _entities[engineId];
            
            // Call remove
            ent.Remove();
            
            // Remove from list
            _entities.Remove(engineId);
        }
        
        #endregion
        
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
            //$"Ticked".Info(ToString());

            // Tick Hooks?
            
            // Tick entities
            // TODO: Do this as an event?
            foreach (var kvp in _entities)
            {
                if (kvp.Value.TickEnabled && kvp.Value.State != EntityState.Unknown)
                    kvp.Value.Tick(ctx.ElapsedTimeFromPreviousFrame.TotalMilliseconds);
            }

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