using Data.Configuration.Poo;
using Data.Model.Items;
using GameServer.Model.Units;

namespace GameServer.Model.Parts.Weapons
{
    /// <summary>
    /// Represents the base class of an in game weapon
    /// </summary>
    public abstract class Weapon : PartBase
    {
        /// <summary>
        /// The arm this weapon is on
        /// </summary>
        public readonly ArmIndex Arm;

        /// <summary>
        /// The weapon set this weapon is equipped to
        /// </summary>
        public readonly WeaponSetIndex WeaponSet;
        
        /// <summary>
        /// Weapon stats
        /// </summary>
        private new WeaponStatsBase Stats => base.Stats as WeaponStatsBase;
        
        protected Weapon(PartRecord partRecord, Unit owner, ArmIndex arm, WeaponSetIndex weaponSet) : base(partRecord, owner)
        {
            Arm = arm;
            WeaponSet = weaponSet;
        }
        
        #region STATE
        
        /// <summary>
        /// The current overheat level of this weapon
        /// </summary>
        public float CurrentOverheat { get; protected set; }
        
        /// <summary>
        /// Is this weapon overheated?
        /// </summary>
        public bool IsOverheated { get; protected set; }
        
        /// <summary>
        /// The step of the combo this weapon is in
        /// Should this be in fist only?
        /// </summary>
        public int ComboStep;

        #endregion

        /// <summary>
        /// Called when the client aims at a unit with this weapon
        /// </summary>
        /// <param name="target"></param>
        /// <returns>Should notify players?</returns>
        public abstract void AimUnit(Unit target);
        
        /// <summary>
        /// Called when the client un-aims with this weapon
        /// </summary>
        /// <returns>Should notify players?</returns>
        public abstract void UnAimUnit();

        /// <summary>
        /// Called when a user wants to fire this weapon
        /// </summary>
        public virtual bool CanAttack()
        {
            // Check overheat
            if (IsOverheated)
                return false;

            return true;
        }

        /// <summary>
        /// Called when the weapon performs an attack
        /// </summary>
        public abstract void OnAttack();

        /// <summary>
        /// Called after an attack is performed
        /// </summary>
        public virtual void PostAttack()
        {
            // Check for overheat
            if (CurrentOverheat >= Owner.Arms.MaxOverheat)
            {
                IsOverheated = true;
                CurrentOverheat = Owner.Arms.MaxOverheat;
                
                // Notify
                Owner.GameInstance.NotifyUnitOverheatStatus(Owner, this);
            }
        }
        
        /// <summary>
        /// Called when the user stops attacking
        /// Seems to be just used for machine guns?
        /// </summary>
        public virtual void OnStopAttack() {}

        /// <summary>
        /// Called when the user dies
        /// </summary>
        public virtual void OnDeath()
        {
            IsOverheated = false;
            CurrentOverheat = 0.0f;
        }
        
        /// <summary>
        /// Tick
        /// </summary>
        /// <param name="delta"></param>
        public virtual void OnTick(double delta)
        {        
            if (CurrentOverheat > 0)
            {
                CurrentOverheat -= (IsOverheated ? Owner.Arms.OverheatRecovery : Stats.OverheatRecovery) * (float)delta / 1000;

                if (CurrentOverheat <= 0)
                {
                    CurrentOverheat = 0;
                    
                    if (IsOverheated)
                    {
                        IsOverheated = false;
                        
                        // Do update
                        Owner.GameInstance.NotifyUnitOverheatStatus(Owner, this);
                    }
                }
            }
        }
        
        public override string ToString()
        {
            return $"{Owner} => [WEAPON]<{Id}:{TemplateId}>";
        }
    }
}