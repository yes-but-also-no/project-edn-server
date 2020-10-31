using System;
using System.Linq;
using Data.Model;
using Data.Model.Items;

namespace GameServer.Game
{
    /*
    public static class StatCalculatorExtensions
    {
        /// <summary>
        /// Calculates all stats required for gameplay on this unit
        /// </summary>
        /// <param name="unit"></param>
        public static void CalculateStats(this UnitRecord unit)
        {
            unit.CalculateMaxHealth();
            unit.CalculateOverheatParameters();
        }
        
        /// <summary>
        /// Calculates the max health and assigns it for a unit
        /// </summary>
        /// <param name="unit"></param>
        private static void CalculateMaxHealth(this UnitRecord unit)
        {
            var headHp = PooReader.Head.First(h => h.TemplateId == unit.Head.TemplateId).HitPoints;
            var chestHp = PooReader.Chest.First(h => h.TemplateId == unit.Chest.TemplateId).HitPoints;
            var armHp = PooReader.Arm.First(h => h.TemplateId == unit.Arms.TemplateId).HitPoints;
            var legHp = PooReader.Leg.First(h => h.TemplateId == unit.Legs.TemplateId).HitPoints;
            var boosterHp = PooReader.Booster.First(h => h.TemplateId == unit.Backpack.TemplateId).HitPoints;

            // calculate
            // TODO: User ability growth
            unit.MaxHealth = headHp + chestHp + armHp + legHp + boosterHp;
            
            // Temp: Set to max hp
            unit.Health = unit.MaxHealth;

//            Console.WriteLine("Calculated max hp of {0} for unit {1}", unit.MaxHealth, unit.Id);
        }

        /// <summary>
        /// Calculates the overheat parameters for this unit
        /// </summary>
        /// <param name="unit"></param>
        private static void CalculateOverheatParameters(this UnitRecord unit)
        {
            var armStats = PooReader.Arm.First(h => h.TemplateId == unit.Arms.TemplateId);
            
            // Calculate weaponset stats
            // TODO: Handle 2h weapons
            unit.WeaponSet1Left.CalculateWeaponParameters(armStats);
            
            // TODO: Check first weapon for type maybe?
            if (unit.WeaponSet1Right != null)
                unit.WeaponSet1Right.CalculateWeaponParameters(armStats);
            
            unit.WeaponSet2Left.CalculateWeaponParameters(armStats);
            
            // TODO: Check first weapon for type maybe?
            if (unit.WeaponSet2Right != null)
                unit.WeaponSet2Right.CalculateWeaponParameters(armStats);
            
        }

        /// <summary>
        /// Calculates the damage and overheat parameters for this weapon
        /// </summary>
        /// <param name="weapon"></param>
        private static void CalculateWeaponParameters(this Weapon weapon, ArmStats armStats)
        {
            WeaponStatsBase weaponStats;

            switch (weapon.Type)
            {
                case 7:
                    weaponStats = PooReader.Gun.First(g => g.TemplateId == weapon.TemplateId);
                    break;
                
                case 6:
                    weaponStats = PooReader.Fist.First(g => g.TemplateId == weapon.TemplateId);
                    break;
                
                // TODO: Error handling here
                default:
                    return;
            }
            
            // Type
            weapon.WeaponType = weaponStats.WeaponType;
            
            // Stats
            weapon.MaxOverheat = armStats.Endurance;
            weapon.OverheatRecovery = armStats.Recovery;
            weapon.Damage = weaponStats.Damage;
            weapon.OverheatPerShot = weaponStats.OverheatPoint;
            weapon.NormalRecovery = weaponStats.OverheatRecovery;
            weapon.IsAutomatic = weaponStats.WeaponType == WeaponType.machingun;
            
            if (weapon.IsAutomatic)
            {
                var gunStats = (GunStats) weaponStats;
                weapon.ReloadTime = gunStats.ReloadTime;
                weapon.NumberOfShots = gunStats.NumberOfShots;
            }
            
            // TODO: This shouldnt be locked to weapon type, in case we want to make new weapons in the future
            var isProjectile = weaponStats.WeaponType == WeaponType.rocket || weaponStats.WeaponType == WeaponType.rifle;
            if (isProjectile)
            {
                var gunStats = (GunStats) weaponStats;
                weapon.IfoType = gunStats.IfoType;
            }

            if (weaponStats.WeaponType == WeaponType.cannon)
            {
                weapon.RecoilDistance = ((GunStats) weaponStats).RecoilDistance;
            }

            if (weaponStats.WeaponType == WeaponType.blade || weaponStats.WeaponType == WeaponType.spear)
            {
                // Melee
                var fistStats = (FistStats) weaponStats;
                weapon.DashDistance = fistStats.DashDistance;
                weapon.AirDashDistance = fistStats.JumpAttackDashDistance;
            }
            
//            Console.WriteLine("Calculated weapon stats: damage {0}, overheat: {1} {2} {3} {4} for weapon {5}", 
//                weapon.Damage, weapon.OverheatPerShot, weapon.NormalRecovery, weapon.MaxOverheat, weapon.OverheatRecovery, weapon.Id);
        }
    }
    */
}