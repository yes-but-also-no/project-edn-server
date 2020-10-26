using System;
using GameServer.Model.Units;
using Console = Colorful.Console;

namespace GameServer.ClientPackets.Game
{
    /// <summary>
    /// Sent when the user stops aiming at a unit
    /// </summary>
    public class UnAimUnit : ClientGameBasePacket
    {
        private readonly ArmIndex _arm;
        
        public UnAimUnit(byte[] data, GameSession client) : base(data, client)
        {
            // Tick
            TickUnit();

            _arm = (ArmIndex) GetInt();
        }

        public override string GetType()
        {
            return "UN_AIM_UNIT";
        }

        protected override void RunImpl()
        {
            Unit?.GetWeaponByArm(_arm).UnAimUnit();
        }
    }
}