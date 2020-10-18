using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Data.Model.Items;
using GameServer.Model.Parts.Skills;
using GameServer.Model.Parts.Skills.AttackSkills;
using GameServer.Model.Parts.Weapons;
using GameServer.Model.Results;
using GameServer.Model.Units;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent when a user uses a skill
    /// </summary>
    public class OnSkill : ServerBasePacket
    {
        private readonly Unit _unit;
        private readonly IEnumerable<HitResult> _hits;
        private readonly WeaponSkill _skill;
        
        // MAX 6!
        // TODO: Make this a struct
        private readonly IEnumerable<Vector3> _dashVectors;

        public OnSkill(Unit unit, IEnumerable<HitResult> hits, WeaponSkill skill, IEnumerable<Vector3> dashVectors)
        {
            _unit = unit;
            _hits = hits;
            _skill = skill;
            _dashVectors = dashVectors;
            
            HighPriority = true;
        }
        
        public override string GetType()
        {
            return "ON_SKILL";
        }

        public override byte GetId()
        {
            return 0x87;
        }

        protected override void WriteImpl()
        {
            WriteInt(GameServer.RunningMs); // Server time
            WriteRaw(new byte[] {0x01, 0x01, 0x01, 0x01}); // Bypass the mapping bc this packet can never miss
            //WriteRaw(_hits.Select(h => (byte)h.ResultCode).ToArray());
            WriteInt(_unit.Id); // Attacker Id
            WriteInt(_skill.Id); // Skill id
            WriteUInt(_skill.TemplateId); // Skill id

            // Successful hits
            var hits = _hits.Where(h => h.ResultCode == HitResultCode.Hit);

            WriteInt(hits.Count()); // Array size
            
            // Write results
            foreach (var hit in hits)
            {
                WriteInt(hit.VictimId);
                WriteInt(hit.Damage);
            }

            foreach (var dashVector in _dashVectors)
            {
                WriteFloat(dashVector.X);
                WriteFloat(dashVector.Y);
                WriteFloat(dashVector.Z);
            }
            
            
        }
    }
}