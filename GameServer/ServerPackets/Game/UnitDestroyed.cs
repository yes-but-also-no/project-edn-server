using GameServer.Model.Parts.Skills;
using GameServer.Model.Parts.Weapons;
using GameServer.Model.Units;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent when someone DIES
    /// </summary>
    public class UnitDestroyed : ServerBasePacket
    {
        private readonly Unit _killer;
        private readonly Unit _victim;
        private readonly Weapon _weapon;
        private readonly Skill _skill;

        public UnitDestroyed(Unit victim, Unit killer, Weapon weapon)
        {
            _killer = killer;
            _victim = victim;
            _weapon = weapon;
        }
        
        public UnitDestroyed(Unit victim, Unit killer, Skill skill)
        {
            _killer = killer;
            _victim = victim;
            _skill = skill;
        }
        
        public override string GetType()
        {
            return "UNIT_DESTROYED";
        }

        public override byte GetId()
        {
            return 0x62;
        }

        protected override void WriteImpl()
        {            
            WriteInt(GameServer.RunningMs); // Unknown
            
            WriteInt(_killer?.Id ?? -1); // Killer
            WriteInt(_victim.Id); // VictimId
            
            WriteUInt(_weapon?.TemplateId ?? 0); // Template of kill weapon   

            WriteUInt(_skill?.TemplateId ?? 0); // 0 for weapon, template for skill?           

            WriteInt(0); // Unknown
        }
    }
}