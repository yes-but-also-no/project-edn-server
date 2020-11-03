using System;
using Engine;
using Engine.Entities;
using Swan.Logging;

namespace Game
{
    /// <summary>
    /// This is the base for a generic scripted entity
    /// </summary>
    public class ScriptedEntity : Entity
    {
        public event Action LuaTick;
        
        public ScriptedEntity(GameEngine engine) : base(engine)
        {
        }

        protected override void OnInitialize()
        {
            // Log
            $"Initialized".Info(ToString());
            
            // Enable tick
            TickEnabled = true;
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
            
            LuaTick?.Invoke();

            // Set next tick
            NextTick = Engine.EngineTime + 1000;
        }
    }
}