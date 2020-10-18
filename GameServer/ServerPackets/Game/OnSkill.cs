using System.Collections.Generic;
using System.Linq;
using Data.Model.Items;
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
        private readonly int _skillId;
        private readonly uint _skillTemplateId;
        
        public OnSkill(Unit unit, int skill, IEnumerable<HitResult> hits, uint template)
        {
            _unit = unit;
            _hits = hits;
            _skillId = skill;
            _skillTemplateId = template;
            
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
            WriteRaw(_hits.Select(h => (byte)h.ResultCode).ToArray());
            WriteInt(_unit.Id); // Attacker Id
            WriteInt(_skillId); // Skill id
            WriteUInt(_skillTemplateId); // Skill id

            // Successful hits
            var hits = _hits.Where(h => h.ResultCode == HitResultCode.Hit);

            WriteInt(hits.Count()); // Array size
            
            // Write results
            foreach (var hit in hits)
            {
                WriteInt(hit.VictimId);
                WriteInt(hit.Damage);
            }

            // Find the hit
            var target = hits.First();

            // If its a miss, end here
            if (target.ResultCode != HitResultCode.Hit)
            {
                return;
            }

            // Find target
            var victim = _unit.GameInstance.GetUnitById(target.VictimId);
            
            // Unknown vectors
            WriteFloat(_unit.WorldPosition.X); // Attacker Start?
            WriteFloat(_unit.WorldPosition.Y); // Unknown
            WriteFloat(_unit.WorldPosition.Z + 200.0f); // Unknown
            
            WriteFloat(_unit.WorldPosition.X); // Attacker Dest?
            WriteFloat(_unit.WorldPosition.Y); // Unknown
            WriteFloat(_unit.WorldPosition.Z); // Unknown

            WriteFloat(0); // Unknown
            WriteFloat(0); // Unknown
            WriteFloat(0); // Unknown
            
            WriteFloat(victim.WorldPosition.X); // Attacker Start?
            WriteFloat(victim.WorldPosition.Y); // Unknown
            WriteFloat(victim.WorldPosition.Z); // Unknown
            
            WriteFloat(victim.WorldPosition.X); // Attacker Start?
            WriteFloat(victim.WorldPosition.Y); // Unknown
            WriteFloat(victim.WorldPosition.Z + 200.0f); // Unknown
            
            WriteFloat(0); // Unknown
            WriteFloat(0); // Unknown
            WriteFloat(0); // Unknown
        }
    }
}