using Data.Model;
using GameServer.Game;

namespace GameServer.Model.Units
{
    /// <summary>
    /// Represents an in game user's unit
    /// </summary>
    public sealed class UserUnit : Unit
    {        
        public UserUnit(GameInstance instance, UnitRecord unitRecord, GameSession owner) : base(instance, unitRecord)
        {
            Owner = owner;
        }

        /// <summary>
        /// TODO: Factor in ability growth / operators / buffs
        /// </summary>
        public override void CalculateStats()
        {
            base.CalculateStats();
        }
    }
}