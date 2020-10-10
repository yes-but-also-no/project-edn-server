using GameServer.Model.Parts.Weapons;
using GameServer.Model.Units;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Looks to include data about the overheat status for a users weapon
    /// </summary>
    public class OverheatStatus : ServerBasePacket
    {
        private readonly Weapon _weapon;
        private readonly Unit _unit;

        public OverheatStatus(Unit unit, Weapon weapon)
        {
            _unit = unit;
            _weapon = weapon;
        }
        
        public override string GetType()
        {
            return "OVERHEAT_STATUS";
        }

        public override byte GetId()
        {
            return 0x83;
        }

        protected override void WriteImpl()
        {
            WriteInt(_unit.Id); // UnitId
            WriteFloat(_weapon.CurrentOverheat); // Overheat value - seems to be absolute...
            WriteInt((int)_weapon.WeaponSet); // WeaponSet
            WriteByte((byte)_weapon.Arm); // Hand
            WriteBool(_weapon.IsOverheated); // Status
        }
    }
}