using Data.Model;
using GameServer.Model.Units;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// I think this is the aim unit packet, but it has a lot of data so i could be wrong
    /// </summary>
    public class UnAimUnit : ServerBasePacket
    {
        private readonly Unit _attacker;
        private readonly Unit _victim;
        private readonly ArmIndex _arm;
        
        public UnAimUnit(Unit attacker, Unit victim, ArmIndex arm)
        {
            _attacker = attacker;
            _victim = victim;
            _arm = arm;
        }
        
        public override string GetType()
        {
            return "UN_AIM_UNIT";
        }

        public override byte GetId()
        {
            return 0x66;
        }

        protected override void WriteImpl()
        {
            WriteInt(GameServer.RunningMs); // Unknown
            WriteInt(_attacker.Id); // UnitID Attacker
            WriteInt(_victim?.Id ?? 0); // UnitId Victim - zero means no lock?
            WriteInt((int)_arm); // Arm Id?
            WriteInt(0); // Unknown - weaponset?
        }
    }
}