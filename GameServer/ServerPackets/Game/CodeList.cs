using System.Collections.Generic;
using System.Linq;
using Data.Model.Items;
using GameServer.Model.Parts.Skills;
using GameServer.Model.Units;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Looks to be a list of all skills in the game
    /// Maybe skills?
    /// </summary>
    public class CodeList : ServerBasePacket
    {
        private readonly IEnumerable<Skill> _codes;

        public CodeList(IEnumerable<Skill> codes)
        {
            _codes = codes;
        }
        
        public override string GetType()
        {
            return "GAME_CODE_LIST";
        }

        public override byte GetId()
        {
            return 0x4a;
        }

        protected override void WriteImpl()
        {
            WriteInt(0); // Always zero?
            
            WriteInt(_codes.Count()); // Array size

            foreach (var code in _codes)
            {
                // Array item
                WriteInt(code.Id); // Id
                WriteUInt(code.TemplateId); // Template
                WriteInt(0); // Unknown
                WriteInt(0); // Unknown
                WriteInt(0); // Unknown
                WriteInt(10000); // Expiry time / current durability
                WriteInt(10000); // Max Durability - Value from server packet capture
                WriteInt(1); // If 1, durability, if 2, expired?
                WriteInt(0); // Unknown
            }
        }
    }
}