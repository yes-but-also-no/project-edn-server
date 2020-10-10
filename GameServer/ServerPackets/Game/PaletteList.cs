using System.Linq;
using GameServer.Model.Units;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Seems to be sent with a list of "palette"
    /// Maybe skills?
    /// </summary>
    public class PaletteList : ServerBasePacket
    {
        /// <summary>
        /// The unit whos data to send
        /// </summary>
        private readonly Unit _unit;

        public PaletteList(Unit unit)
        {
            _unit = unit;
        }
        
        public override string GetType()
        {
            return "GAME_PALETTE_LIST";
        }

        public override byte GetId()
        {
            return 0x4b;
        }

        protected override void WriteImpl()
        {
            WriteInt(_unit.Id);
            
            WriteInt(4); // Slot count?

            var skills = _unit.GetSkills();
            
            WriteInt(skills.Count()); // Code count?

            foreach (var skill in skills)
            {
                WriteInt(skill);
            }
        }
    }
}