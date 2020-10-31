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

        private readonly GameSession _session;

        public UnitInfo(Unit unit, GameSession session)
        {
            HighPriority = true;
            
            _unit = unit;
            _session = session;
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

            var team = _unit.Team;

            // Deathmatch? no teams?
            if (team == 0xffffffff)
            {
                // We will write 0 for this user, 1 for others
                team = _session.UserId == _unit.Owner.UserId ? (uint) 0 : (uint) 1;
            }
            
            WriteUInt(team); // Team
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