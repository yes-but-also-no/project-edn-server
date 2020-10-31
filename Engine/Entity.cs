using System;
using Microsoft.Xna.Framework;

namespace Engine
{
    /// <summary>
    /// This is the entity for any spawnable game object
    /// Entities will be created by the engine, and can be spawned / despawned multiple times
    /// It is very rare that an entity will be destroyed, but it can happen
    /// </summary>
    public abstract class Entity
    {
        #region ENGINE

        /// <summary>
        /// The game engine reference for this entity
        /// </summary>
        protected readonly GameEngine Engine;
        
        /// <summary>
        /// The unique Id of this entity
        /// ENGINE ONLY
        /// </summary>
        public readonly Guid EngineId = Guid.NewGuid();
        
        /// <summary>
        /// The name of this entity
        /// ENGINE ONLY
        /// </summary>
        public string EngineName { get; protected set; }
        
        /// <summary>
        /// The class of this entity
        /// ENGINE ONLY
        /// </summary>
        public string EngineClass { get; protected set; }
        
        #endregion
        
        #region STATE
        
        /// <summary>
        /// The gameplay state of this entity
        /// </summary>
        public EntityState State { get; private set; }
        
        /// <summary>
        /// Entity world location
        /// </summary>
        public Vector3 Location { get; protected set; }

        /// <summary>
        /// Entity world rotation
        /// </summary>
        public Vector3 Rotation { get; protected set; }

        /// <summary>
        /// Entity world velocity
        /// </summary>
        public Vector3 Velocity { get; protected set; }
        
        #endregion

        #region TICK

        /// <summary>
        /// Controls when this entity should run its tick next
        /// </summary>
        public int NextTick { get; protected set; }
        
        /// <summary>
        /// Controls if this entity should tick at all
        /// TODO: Should these change a subscription / hook against the game server?
        /// </summary>
        public bool TickEnabled { get; protected set; }

        #endregion
        
        #region INTERNAL METHODS

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="engine"></param>
        internal Entity(GameEngine engine)
        {
            // Store engine ref
            Engine = engine;
        }

        /// <summary>
        /// First time initialization
        /// </summary>
        internal void Initialize()
        {
            // TODO: Register with engine?
            
            // Set state
            State = EntityState.OutOfPlay;
            
            // Call hook
            OnInitialize();
        }

        /// <summary>
        /// Removes this entity, destroying it completely
        /// </summary>
        internal void Remove()
        {
            // Set state
            State = EntityState.Removed;
            
            // Call hook
            OnRemoved();
            
            // TODO: Deregister with engine?
        }

        /// <summary>
        /// Ticks this entity
        /// </summary>
        internal void Tick(double delta)
        {
            // Check if we want to tick
            if (NextTick > 0 && Engine.EngineTime < NextTick)
                return;
            
            // Reset next tick
            NextTick = 0;
            
            // Call hook
            OnTick(delta);
        }
        
        #endregion
        
        #region PUBLIC METHODS

        /// <summary>
        /// Spawns this entity into the game engine
        /// </summary>
        public void Spawn()
        {
            // Set state
            State = EntityState.InPlay;
            
            // Call hook
            OnSpawn();
            
            // TODO: Register tick / events?
        }

        /// <summary>
        /// DeSpawns this entity from the game engine, but does not destroy it
        /// </summary>
        public void DeSpawn()
        {
            // Set state
            State = EntityState.OutOfPlay;
            
            // Call hook
            OnDeSpawn();
            
            // TODO: DeRegister tick / events?
        }
        
        #endregion
        
        #region HOOKS

        /// <summary>
        /// Called when this entity is created for the first time
        /// </summary>
        protected abstract void OnInitialize();
        
        /// <summary>
        /// Called when this entity is spawned
        /// </summary>
        protected abstract void OnSpawn();
        
        /// <summary>
        /// Called when this object is de-spawned / killed
        /// </summary>
        protected abstract void OnDeSpawn();

        /// <summary>
        /// Called when this entity is removed
        /// </summary>
        protected abstract void OnRemoved();

        /// <summary>
        /// Called every scheduled tick, if this entity has ticks enabled
        /// Next call is controlled by NextTick
        /// </summary>
        protected abstract void OnTick(double delta);

        #endregion
    }
    
    /// <summary>
    /// The state of an entity
    /// </summary>
    public enum EntityState
    {
        Unknown,
        OutOfPlay,
        //Spawning,
        InPlay,
        //DeSpawning,
        Removed
    }
}