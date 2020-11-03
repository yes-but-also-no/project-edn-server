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
        /// <summary>
        /// Delegate for ticks
        /// </summary>
        public event Action<double> LuaTick;
        
        public event Action LuaOnSpawn;
        
        public event Action LuaOnDeSpawn;
        
        public event Action LuaOnRemoved;

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
        
        public ScriptedEntity(GameEngine engine) : base(engine)
        {
        }

        protected override void OnInitialize()
        {
            // Log
            $"Initialized".Info(ToString());
            
            // Hook is called in lua, since this function is called directly by the C# engine
        }

        protected override void OnSpawn()
        {
            // Log
            $"Spawned".Info(ToString());
            
            // Call lua hook
            LuaOnSpawn?.Invoke();
        }

        protected override void OnDeSpawn()
        {
            // Log
            $"DeSpawned".Info(ToString());
            
            // Call lua hook
            LuaOnDeSpawn?.Invoke();
        }

        protected override void OnRemoved()
        {
            // Log
            $"Removed".Info(ToString());
            
            // Call lua hook
            LuaOnRemoved?.Invoke();
        }

        protected override void OnTick(double delta)
        {
            // Log
            $"Ticked".Info(ToString());
            
            // Call lua hook
            LuaTick?.Invoke(delta);
        }
    }
}