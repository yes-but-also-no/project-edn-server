using System.Numerics;
using GameServer.Model.Units;
using Console = Colorful.Console;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent to indicate that a unit has moved
    /// </summary>
    public class UnitMoved : ServerBasePacket
    {
        private readonly Unit _unit;
        private readonly Vector3 _vel;

        public UnitMoved(Unit unit, Vector3 vel)
        {
            HighPriority = true;
            
            _unit = unit;
            _vel = vel;
        }
        
        public override string GetType()
        {
            return "GAME_UNIT_MOVED";
        }

        public override byte GetId()
        {
            return 0x63;
        }

        protected override void WriteImpl()
        {
            WriteInt(GameServer.RunningMs); // Server time
            WriteInt(_unit.Id); 
            
            WriteByte(_unit.Movement);
            WriteByte(_unit.UnknownMovementFlag);
            WriteByte(_unit.Boosting);
            
            WriteFloat(_unit.WorldPosition.X);
            WriteFloat(_unit.WorldPosition.Y);
            WriteFloat(_unit.WorldPosition.Z);
            
            WriteFloat(_vel.X); // Velocity X
            WriteFloat(_vel.Y); // Velocity Y
            WriteFloat(_vel.Z); // Velocity Z
            
            WriteUShort(_unit.AimY);
            WriteUShort(_unit.AimX);
        }
    }
}