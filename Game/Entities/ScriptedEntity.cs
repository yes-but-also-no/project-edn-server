using System;
using Engine;
using Engine.Entities;
using Swan.Logging;

namespace Game.Entities
{
    /// <summary>
    /// This is the base for a generic scripted entity
    /// </summary>
    public class ScriptedEntity : Entity
    {
        #region DELEGATES
        
        /// <summary>
        /// Delegate for ticks
        /// </summary>
        public event Action<double> LuaTick;
        
        /// <summary>
        /// Delegate for spawn hook
        /// </summary>
        public event Action LuaOnSpawn;
        
        /// <summary>
        /// Delegate for despawn hook
        /// </summary>
        public event Action LuaOnDeSpawn;
        
        /// <summary>
        /// Delegate for removed hook
        /// </summary>
        public event Action LuaOnRemoved;
        
        #endregion

        #region GETTERS AND SETTERS
        
        /// <summary>
        /// Lua accessor for id
        /// </summary>
        public string GetEngineId() => EngineId.ToString();

        /// <summary>
        /// Lua accessor for tick enable
        /// </summary>
        /// <param name="enabled"></param>
        public void SetTickEnabled(bool enabled) => TickEnabled = enabled;

        /// <summary>
        /// Lua accessor for next tick
        /// </summary>
        /// <param name="nextTick"></param>
        public void SetNextTick(int nextTick) => NextTick = nextTick;

        /// <summary>
        /// Is this entity spawned yet
        /// </summary>
        /// <returns></returns>
        public bool IsSpawned() => State == EntityState.InPlay;
        
        #endregion
        
        public ScriptedEntity(GameEngine engine) : base(engine)
        {
        }

        protected override void OnInitialize()
        {
            // Log
            $"Initialized".Debug(ToString());
            
            // Hook is called in lua, since this function is called directly by the C# engine
        }

        protected override void OnSpawn()
        {
            // Log
            $"Spawned".Debug(ToString());
            
            // Call lua hook
            LuaOnSpawn?.Invoke();
        }

        protected override void OnDeSpawn()
        {
            // Log
            $"DeSpawned".Debug(ToString());
            
            // Call lua hook
            LuaOnDeSpawn?.Invoke();
        }

        protected override void OnRemoved()
        {
            // Log
            $"Removed".Debug(ToString());
            
            // Call lua hook
            LuaOnRemoved?.Invoke();
        }

        protected override void OnTick(double delta)
        {
            // Log
            //$"Ticked".Debug(ToString());
            
            // Call lua hook
            LuaTick?.Invoke(delta);
        }
    }
}