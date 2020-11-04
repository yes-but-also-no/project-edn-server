using Engine;
using Engine.Entities;
using Swan.Logging;

namespace Game.Entities
{
    public class TestEntity : Entity
    {
        public TestEntity(GameEngine engine) : base(engine)
        {
        }

        protected override void OnInitialize()
        {
            // Log
            $"Initialized".Debug(ToString());
            
            // Enable tick
            TickEnabled = true;
        }

        protected override void OnSpawn()
        {
            // Log
            $"Spawned".Debug(ToString());
        }

        protected override void OnDeSpawn()
        {
            // Log
            $"DeSpawned".Debug(ToString());
        }

        protected override void OnRemoved()
        {
            // Log
            $"Removed".Debug(ToString());
        }

        protected override void OnTick(double delta)
        {
            // Log
            //$"Ticked".Debug(ToString());

            // Set next tick
            NextTick = Engine.EngineTime + 1000;
        }

        
    }
}