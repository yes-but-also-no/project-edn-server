using Engine;
using Engine.Entities;
using Swan.Logging;

namespace Game
{
    public class TestEntity : Entity
    {
        public TestEntity(GameEngine engine) : base(engine)
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

            // Set next tick
            NextTick = Engine.EngineTime + 1000;
        }

        
    }
}