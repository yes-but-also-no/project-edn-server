using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Model.Items
{
    /// <summary>
    /// Represents a single weapon in the game
    /// Mostly used in runtime
    /// </summary>
    public class Weapon : PartRecord
    {
        

        /// <summary>
        /// TODO: Calculate this
        /// </summary>
        [NotMapped]
        public int Damage = 1;

        

        
        // TODO: Load from files
        public float OverheatPerShot = 1.0f; // From weapon
        public float NormalRecovery = 1.0f; // From weapon
        public float OverheatRecovery = 1.0f; // From arms of unit
        public float MaxOverheat = 1.0f; // From arms of unit
        
        // Automatic weapons
        public bool IsAutomatic = false;
        public float ReloadTime = 1.0f; // Used for automatic weapons
        public bool IsAttacking = false;
        public float CurrentReloadTime = 0.0f;
        public int NumberOfShots = 1;
        public float RecoilDistance = 0.0f;

        public float DashDistance = 0.0f;
        public float AirDashDistance = 0.0f;
        
        // Melee weapons
        public int ComboStep = 0;
        
        // Projectile weapons


        /// <summary>
        /// Called when the weapon is fired to increase its reload timer
        /// </summary>
        public void AddReloadTime()
        {
//            Console.WriteLine("Added {0} reload time", ReloadTime);
            CurrentReloadTime = ReloadTime;
        }
        
        /// <summary>
        /// Checks if the weapon is ready to fire again
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public bool ShouldAttack(float delta)
        {
            if (IsAttacking)
            {
                if (CurrentReloadTime <= 0)
                {
                    // Yes, attack
                    return true;
                }
                else
                {
                    CurrentReloadTime = CurrentReloadTime - ReloadTime * delta / 1000;
                }
            }

            return false;
        }

        /// <summary>
        /// Adds overheat to the weapon
        /// TODO: Calculate this
        /// </summary>
//        public bool AddOverheat()
//        {
//            CurrentOverheat += OverheatPerShot;
//
//            // TODO: From unit config?
//            if (CurrentOverheat >= MaxOverheat)
//            {
//                IsOverheated = true;
//                IsAttacking = false;
//                CurrentOverheat = MaxOverheat;
//
//                return true;
//            }
//
//            return false;
//        }

        /// <summary>
        /// Tick overheat and see if we need to update the client
        /// </summary>
        /// <param name="delta">Time in MS since last tick</param>
//        public bool ShouldUpdateOverheat(float delta)
//        {
//            if (CurrentOverheat > 0)
//            {
//                CurrentOverheat -= (IsOverheated ? OverheatRecovery : NormalRecovery) * delta / 1000;
//
//                if (CurrentOverheat <= 0)
//                {
//                    CurrentOverheat = 0;
//                    
//                    if (IsOverheated)
//                    {
//                        IsOverheated = false;
//                        
//                        // Do update
//                        return true;
//                    }
//                }
//            }
//
//            // Do not update
//            return false;
//        }
    }
    
    
}