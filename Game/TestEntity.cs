using System;
using Engine;
using Engine.Entities;
using MoonSharp.Interpreter;
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
            
            string script = @"    
		-- defines a factorial function
		function fact (n)
			if (n == 0) then
				return 1
			else
				return n*fact(n - 1)
			end
		end

		return fact(5)";

            DynValue res = Script.RunString(script);
            
            Console.WriteLine(res.Number);
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