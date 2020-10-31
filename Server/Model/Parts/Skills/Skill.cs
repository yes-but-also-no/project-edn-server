using System;
using System.Collections.Generic;
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
        protected new CodeStats Stats => base.Stats as CodeStats;
        
        public new uint TemplateId => Stats.TemplateId;
        
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
            // Check SP
            if (Owner.CurrentSp < Stats.RequiredSp)
                return false;
            
            // Check cooldown
            if (Cooldown > 0)
                return false;

            return true;
        }

        /// <summary>
        /// Called when the skill is used
        /// </summary>
        public abstract void OnUse(IEnumerable<Unit> target);

        /// <summary>
        /// Called after the skill is used
        /// </summary>
        public virtual void PostUse()
        {
            // Add cooldown
            Cooldown = Stats.CoolTime;
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
            if (Cooldown > 0)
            {
                Cooldown -= MathF.Max((float)delta / 1000, 0);
            }
        }
        
        public override string ToString()
        {
            return $"{Owner} => [SKILL]<{Id}:{TemplateId}>";
        }
    }
}