using GameServer.Model.Units;
using Console = Colorful.Console;

namespace GameServer.ClientPackets.Game
{
    /// <summary>
    /// Sent when a user aims on a unit
    /// Used to give them a "locked" notification i believe
    /// </summary>
    public class AimUnit : ClientGameBasePacket
    {
        // TODO: Maybe swap this to a lookup of the victim unit?
        // TODO: Maybe a units dictionary in the room?
        private readonly int _target;
        private readonly ArmIndex _arm;
        
        public AimUnit(byte[] data, GameSession client) : base(data, client)
        {
            TickUnit();

            // Find the target unit
            _target = GetInt();
            
            // Find which arm is being used
            _arm = (ArmIndex) GetInt();
            
            // Read the units position and aim
            GetUnitPositionAndAim();
        }

        public override string GetType()
        {
            return "AIM_UNIT";
        }

        protected override void RunImpl()
        {
            Unit?.TryAimUnit(_arm, _target);
        }
    }
}