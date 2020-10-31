using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Data.Model.Items;
using GameServer.Configuration.Poo;
using GameServer.Model.Results;
using GameServer.Model.Units;
using GameServer.Util;

namespace GameServer.Model.Parts.Weapons
{
    public class Fist : Weapon
    {
        /// <summary>
        /// Stats for this fist
        /// </summary>
        private new FistStats Stats => base.Stats as FistStats;

        
        
        public Fist(PartRecord partRecord, Unit owner, ArmIndex arm, WeaponSetIndex weaponSet) : base(partRecord, owner, arm, weaponSet)
        {
        }

        public override void AimUnit(Unit target)
        {
            // Swords do not aim for now?
        }

        public override void UnAimUnit()
        {
            // Swords do not aim for now?
        }

        public override void OnAttack()
        {
            // Apply overheat
            CurrentOverheat += Stats.OverheatPoint;
            
            // Calculate hits
            
            // Find pos of owners head
            var startPos = Owner.GetBodyPosition();
            
            // Find the direction they are aiming
            // Only include Z axis if they are in air, otherwise lateral attack
            var direction = Owner.GetAimDirection(Owner.InAir);
            
            // Get angle of attack
            var angle = Owner.InAir ? Stats.JumpAttackAngle : Stats.LockOnAngle;
            
            // Get distance of attack
            var distance = 0.0f;

            // In air we use air dash
            if (Owner.InAir)
            {
                distance += Stats.JumpAttackDashDistance;
            }
            // On ground, we use distance of dash, or both if dual weild
            else
            {
                distance += ComboStep <= 2 ? Stats.DashDistance : Stats.BothDashDistance;
            }
            
            // Hit results
            var hits = new List<HitResult>();
            
            // Calculate hits
            var first = GameInstance.GetEnemies(Owner)
                .CheckInCone(startPos, direction, angle, distance + Stats.Range) // TODO: Check iteratively?
                .FirstOrDefault(); 

            // If we hit, check for splash
            if (first != null)
            {
                var damage = ComboStep >= 2 ? Stats.BothDamage : Stats.Damage;
                
                // Add first hit
                hits.Add(new HitResult
                {
                    VictimId = first.Id,
                    Damage = damage,
                    PushBack = Owner.InAir ? Vector3.Zero : direction * distance, // TODO: Is this thrust?
                    ResultCode = HitResultCode.Hit
                });
                
                // Apply damage
                first.Attack(this, damage);
                
                // Check for splash
                var splash = GameInstance.GetEnemies(Owner)
                    .CheckInCone(startPos, direction, Stats.SplashAngle, distance + Stats.Range) // TODO: Check iteratively?
                    .Where(u => u != first)
                    .Take(Stats.SplashCount); // TODO: Take based on ability growth

                foreach (var unit in splash)
                {
                    // Add each
                    hits.Add(new HitResult
                    {
                        VictimId = unit.Id,
                        Damage = Stats.SplashDamage,
                        PushBack = Vector3.Zero,
                        ResultCode = HitResultCode.Hit
                    });
                    
                    // Apply damage
                    unit.Attack(this, Stats.SplashDamage);
                }
            }
            else
            {
                // Missed all
                hits.AddRange(new [] { HitResult.Miss, HitResult.Miss, HitResult.Miss, HitResult.Miss });
            }
            
            // Calculate new position (Only uses dash distance, excludes range)
            // TODO: Stop early on hit?
            var newPos = Owner.WorldPosition + direction * distance;
            
            // Calculate new pos
            Vector3 movePos;

            if (Owner.InAir)
                movePos = GameInstance.Map.MoveCheck3D(Owner.WorldPosition, newPos);
            else
                movePos = GameInstance.Map.MoveCheck(Owner.WorldPosition, newPos);
            
            // Apply
            Owner.WorldPosition = movePos;
            
            // Notify
            GameInstance.NotifyUnitAttacked(Owner, hits.ToList(), this);
        }
    }
}