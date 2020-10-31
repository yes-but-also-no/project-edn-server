using GameServer.Model.Units;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent when a unit stops? Not sure the purpose
    /// </summary>
    public class UnitStopped : ServerBasePacket
    {
        private readonly Unit _unit;

        public UnitStopped(Unit unit)
        {
            _unit = unit;
        }
        
        public override string GetType()
        {
            return "STOP_UNIT";
        }

        public override byte GetId()
        {
            return 0x64;
        }

        protected override void WriteImpl()
        {
            WriteInt(GameServer.RunningMs); // Unknown
            WriteInt(_unit.Id); // Unknown - unitId?
            
            WriteByte(_unit.Movement); // Unknown
            WriteByte(_unit.UnknownMovementFlag); // Unknown
            
            WriteFloat(_unit.WorldPosition.X);
            WriteFloat(_unit.WorldPosition.Y);
            WriteFloat(_unit.WorldPosition.Z);
            
            WriteFloat(0); // Unknown - vector?
            WriteFloat(0); // Unknown - vector?
            WriteFloat(0); // Unknown - vector?
            
            WriteUShort(_unit.AimY);
            WriteUShort(_unit.AimX);
            
            WriteInt(0); // Reason
        }
    }
}