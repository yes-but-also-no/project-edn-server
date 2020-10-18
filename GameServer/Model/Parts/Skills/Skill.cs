using Data.Model.Items;
using GameServer.Configuration.Poo;
using GameServer.Model.Units;

namespace GameServer.Model.Parts.Skills
{
    /// <summary>
    /// Represents the base class of an in game skill
    /// </summary>
    public abstract class Skill : PartBase
    {
        
        /// <summary>
        /// Weapon stats
        /// </summary>
        private new WeaponStatsBase Stats => base.Stats as WeaponStatsBase;
        
        protected Skill(PartRecord partRecord, Unit owner) : base(partRecord, owner)
        {
        }
        
        #region STATE
        
        /// <summary>
        /// The current cooldown of this skill
        /// </summary>
        public float Cooldown { get; protected set; }

        #endregion

        /// <summary>
        /// Called when a user wants to use this skill
        /// </summary>
        public virtual bool CanUse()
        {
            // Check EN
            
            // Check cooldown

            return true;
        }

        /// <summary>
        /// Called when the skill is used
        /// </summary>
        public abstract void OnUse();

        /// <summary>
        /// Called after the skill is used
        /// </summary>
        public virtual void PostUse()
        {
            // Add cooldown
        }

        /// <summary>
        /// Called when the user dies
        /// </summary>
        public virtual void OnDeath()
        {
        }
        
        /// <summary>
        /// Tick
        /// </summary>
        /// <param name="delta"></param>
        public virtual void OnTick(double delta)
        {        
            // Reduce cooldown
        }
        
        public override string ToString()
        {
            return $"{Owner} => [SKILL]<{Id}:{TemplateId}>";
        }
    }
}