using GameServer.Model.Units;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// I think this is the aim unit packet, but it has a lot of data so i could be wrong
    /// </summary>
    public class AimUnit : ServerBasePacket
    {
        private readonly Unit _attacker;
        private readonly Unit _victim;
        private readonly ArmIndex _arm;
        
        public AimUnit(Unit attacker, Unit victim, ArmIndex arm)
        {
            _attacker = attacker;
            _victim = victim;
            _arm = arm;
        }
        
        public override string GetType()
        {
            return "AIM_UNIT";
        }

        public override byte GetId()
        {
            return 0x65;
        }

        protected override void WriteImpl()
        {
            WriteInt(GameServer.RunningMs); // Unknown
            WriteInt(_attacker.Id); // UnitID Attacker
            WriteInt(_victim.Id); // UnitId Victim - zero means no lock?
            WriteInt((int)_arm); // Arm?
            WriteInt(0); // Unknown - weapon set?
            
            WriteUShort(_attacker.AimY); // Attacker - AimX
            WriteUShort(_attacker.AimX); // Attacker - AimY
            
            WriteFloat(_attacker.WorldPosition.X); // Attacker - X
            WriteFloat(_attacker.WorldPosition.Y); // Attacker - Y
            WriteFloat(_attacker.WorldPosition.Z); // Attacker - Z
        }
    }
}