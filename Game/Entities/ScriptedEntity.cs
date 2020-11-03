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

        /// <summary>
        /// Lua accessor for id
        /// </summary>
        public string GetEngineId()
        {
            Console.WriteLine("hi");
            return EngineId.ToString();
        }
        
        public ScriptedEntity(GameEngine engine) : base(engine)
        {
        }

        protected override void OnInitialize()
        {
            // Log
            $"Initialized".Info(ToString());
        }

        protected override void OnSpawn()
        {
            // Log
            $"Spawned".Info(ToString());
        }

        protected override void OnDeSpawn()
        {
            // Log
            $"DeSpawned".Info(ToString());
        }

        protected override void OnRemoved()
        {
            // Log
            $"Removed".Info(ToString());
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