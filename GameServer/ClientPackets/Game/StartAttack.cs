using System;
using System.Drawing;
using System.Linq;
using GameServer.Model.Units;
using GameServer.ServerPackets.Game;
using Console = Colorful.Console;

namespace GameServer.ClientPackets.Game
{
    /// <summary>
    /// Sent when a user begins attacking
    /// </summary>
    public class StartAttack : ClientGameBasePacket
    {
        /// <summary>
        /// The arm they are attacking with
        /// </summary>
        private readonly ArmIndex _arm;

        /// <summary>
        /// For melee weapons, what combo step they are on
        /// </summary>
        private readonly int _comboStep;
        
        public StartAttack(byte[] data, GameSession client) : base(data, client)
        {
            TickUnit();

            _arm = (ArmIndex) GetInt();
            _comboStep = GetInt();
            
            // Read the position information
            GetUnitPositionAndAim();
            
            // High priority
            HighPriority = true;
        }

        public override string GetType()
        {
            return "START_ATTACK";
        }

        protected override void RunImpl()
        {
            // try to attack
            Unit?.TryAttack(_arm, _comboStep);
        }
    }
}