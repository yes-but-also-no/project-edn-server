using GameServer.Model.Units;

namespace GameServer.ClientPackets.Game
{
    /// <summary>
    /// Sent when the user stops attacking with an automatic weapon
    /// </summary>
    public class StopAttack : ClientGameBasePacket
    {
        /// <summary>
        /// The arm they are using
        /// </summary>
        private readonly ArmIndex _arm;
        
        public StopAttack(byte[] data, GameSession client) : base(data, client)
        {
            _arm = (ArmIndex) GetInt();
            
            // Standard position update
            GetUnitPositionAndAim();
        }

        public override string GetType()
        {
            return "STOP_ATTACK";
        }

        protected override void RunImpl()
        {
            var weapon = Unit?.GetWeaponByArm(_arm);

            // For machine guns
            weapon?.OnStopAttack();
        }
    }
}