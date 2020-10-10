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
            try
            {
//            debug = true;

                // Check practice mode
                if (GetClient().GameInstance == null) return;

                TickUnit();

                _arm = (ArmIndex) GetInt();
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Exception in Unaim unit: " + e.Message);
            }
        }

        public override string GetType()
        {
            return "UN_AIM_UNIT";
        }

        protected override void RunImpl()
        {
            // Check practice mode
            if (GetClient().GameInstance == null) return;

            Unit.GetWeaponByArm(_arm).UnAimUnit();
        }
    }
}