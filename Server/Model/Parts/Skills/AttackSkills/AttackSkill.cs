using System.Linq;
using Data.Configuration;
using Data.Configuration.Poo;
using Data.Model.Items;
using GameServer.Model.Units;

namespace GameServer.Model.Parts.Skills.AttackSkills
{
    /// <summary>
    /// Base stats for attack skills
    /// </summary>
    public abstract class AttackSkill : Skill
    {
        /// <summary>
        /// The attack specific stats for this skill
        /// </summary>
        protected readonly AttackSkillStats AttackStats;
        
        protected AttackSkill(PartRecord partRecord, Unit owner) : base(partRecord, owner)
        {
            AttackStats = PooReader.AttackSkill.First(a => a.TemplateId == Stats.AttackSkill);
        }
    }
}