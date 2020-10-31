using GameServer.Model.Units;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent when a user switches weapon sets
    /// </summary>
    public class WeaponsetChanged : ServerBasePacket
    {
        private readonly Unit _unit;
        
        public WeaponsetChanged(Unit unit)
        {
            _unit = unit;
        }
        
        public override string GetType()
        {
            return "WEAPONSET_CHANGED";
        }

        public override byte GetId()
        {
            return 0x84;
        }

        protected override void WriteImpl()
        {
            WriteInt(_unit.Id); // Unit Id
            WriteInt((int)_unit.WeaponSet); // Slot
            WriteFloat(_unit.GetWeaponByArm(ArmIndex.Left).CurrentOverheat); // Current overheat
            WriteFloat(_unit.GetWeaponByArm(ArmIndex.Right).CurrentOverheat); // Current overheat
            
            WriteBool(_unit.GetWeaponByArm(ArmIndex.Left).IsOverheated); // Is overheated
            WriteBool(_unit.GetWeaponByArm(ArmIndex.Right).IsOverheated); // Is overheated
        }
    }
}