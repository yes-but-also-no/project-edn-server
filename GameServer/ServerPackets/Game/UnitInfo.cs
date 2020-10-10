using Data.Model;
using GameServer.Model.Units;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent in game with data about users units
    /// </summary>
    public class UnitInfo : ServerBasePacket
    {
        private readonly Unit _unit;

        public UnitInfo(Unit unit)
        {
            HighPriority = true;
            
            _unit = unit;
        }
        
        public override string GetType()
        {
            return "GAME_UNIT_INFO";
        }

        public override byte GetId()
        {
            return 0x4c;
        }

        protected override void WriteImpl()
        {
            WriteInt(_unit.UserId);
            WriteInt(_unit.Id);
            
            WriteUInt(_unit.Team); // Team
            WriteInt(_unit.CurrentHealth); // Unit HP?
            
            WriteString(_unit.Name);
            
            this.WritePartInfo(_unit.Head);
            this.WritePartInfo(_unit.Chest);
            this.WritePartInfo(_unit.Arms);
            this.WritePartInfo(_unit.Legs);
            this.WritePartInfo(_unit.Booster);
            
            this.WritePartInfo(_unit.WeaponSetPrimary.Left);
            this.WritePartInfo(_unit.WeaponSetPrimary.Right);
            
            this.WritePartInfo(_unit.WeaponSetSecondary.Left);
            this.WritePartInfo(_unit.WeaponSetSecondary.Right);
        }
    }
}