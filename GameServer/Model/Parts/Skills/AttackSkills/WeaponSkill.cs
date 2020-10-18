using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Data.Model.Items;
using GameServer.Configuration.Poo;
using GameServer.Model.Results;
using GameServer.Model.Units;
using GameServer.Util;

namespace GameServer.Model.Parts.Skills.AttackSkills
{
    /// <summary>
    /// This represents a standard weapon skill
    /// </summary>
    public class WeaponSkill : AttackSkill
    {
        public WeaponSkill(PartRecord partRecord, Unit owner) : base(partRecord, owner)
        {
        }

        /// <summary>
        /// Called when the skill is triggered
        /// </summary>
        /// <param name="target"></param>
        public override void OnUse(IEnumerable<Unit> skillTargets)
        {
            // List for storing results
            var hits = new List<HitResult>();
            
            // Units to check against
            // Note: only one skill targets team mates, double ENG. But according to stats, 
            // it uses team type of enemy, so it is overriden in Config/PooReader.cs
            // It is left in place so that we can create self team targeting skills in the future
            var targets = AttackStats.TargetTeam == TargetTeam.enemy
                ? GameInstance.GetEnemies(Owner)
                : GameInstance.GetFriends(Owner);
            
            // Determine if skill is self targeted or enemy
            
            // If self...
            if (Stats.CodeActivationTarget == CodeActivationTarget.self)
            {
                // If targeting "near"
                if (AttackStats.TargetType == TargetType.near)
                {
                    // Find near by targets
                    var near = targets
                        .CheckInSphere(Owner.WorldPosition, AttackStats.TargetDistance)
                        .Take(AttackStats.MaxTarget);

                    // Attack them all
                    foreach (var unit in near)
                    {
                        // Add each
                        hits.Add(new HitResult
                        {
                            VictimId = unit.Id,
                            // Temp - one or the other. Eventually could be cool do both (maybe: heal / poison grenade?)
                            Damage = AttackStats.Heal > 0 ? AttackStats.Heal : AttackStats.Damage,
                            ResultCode = HitResultCode.Hit
                        });
                        
                        // Apply damage
                        // TODO: HEALING - SENDING NEGATIVE DAMAGE FOR NOW?
                        unit.Attack(this, AttackStats.Heal > 0 ? -AttackStats.Heal : AttackStats.Damage);
                    }
                }
                // TODO: TargetType.front - looks to be used by the unused spear - sword skill
            } 
            // If enemy, perfrom attack
            else if (Stats.CodeActivationTarget == CodeActivationTarget.enemy)
            {
                foreach (var target in skillTargets)
                {
                    // Add each
                    hits.Add(new HitResult
                    {
                        VictimId = target.Id,
                        // Temp - one or the other. Eventually could be cool do both (maybe: heal / poison grenade?)
                        Damage = AttackStats.Heal > 0 ? AttackStats.Heal : AttackStats.Damage,
                        ResultCode = HitResultCode.Hit
                    });
                    
                    // Apply damage
                    // TODO: HEALING - SENDING NEGATIVE DAMAGE FOR NOW?
                    target.Attack(this, AttackStats.Heal > 0 ? -AttackStats.Heal : AttackStats.Damage);
                    
                    // If healing, leave here
                    if (AttackStats.Heal > 0) continue;

                    // Check for any splash damage
                    // TODO: Right now, its only checking for presence of splash damage,
                    // but it should be using the target type of activator | splash
                    if (AttackStats.SplashDamage > 0 && AttackStats.SplashRadius > 0)
                    {
                        var near = targets
                            .CheckInSphere(target.WorldPosition, AttackStats.SplashRadius);
                        
                        // Attack them all
                        foreach (var unit in near)
                        {
                            // Add each
                            hits.Add(new HitResult
                            {
                                VictimId = unit.Id,
                                // Temp - one or the other. Eventually could be cool do both (maybe: heal / poison grenade?)
                                Damage = AttackStats.SplashDamage,
                                ResultCode = HitResultCode.Hit
                            });
                            
                            // Apply damage
                            unit.Attack(this, AttackStats.SplashDamage);
                        }
                    }
                }
            }
            
            // Create dash vectors if needed
            var dashVectors = new List<Vector3>
            {
                new Vector3(0, 0, -1),
                new Vector3(0, 0, -1),
                new Vector3(0, 0, -1),
                new Vector3(0, 0, -1),
                new Vector3(0, 0, -1),
                new Vector3(0, 0, -1),
            };

            // If its a single target
            if (AttackStats.TargetType == TargetType.activator)
            {
                // Get victim
                var target = targets.First();
                
                // Get direction
                var direction = Vector3.Normalize(target.WorldPosition - Owner.WorldPosition);
                
                // Iterator
                var i = 0;
                
                // Do attacker offsets
                foreach (var dash in AttackStats.AttackerDashDistances.Where(d => d != -1))
                {
                    // Figure offset
                    var calc = Vector3.Add(Owner.WorldPosition, Vector3.Multiply(dash, direction));
                    
                    // Add to vector array
                    dashVectors[i] = new Vector3(calc.X, calc.Y, Owner.WorldPosition.Z);

                    // Just update every time for now i guess
                    Owner.WorldPosition = new Vector3(dashVectors[i].X, dashVectors[i].Y, dashVectors[i].Z);

                    i++;
                }

                i = 3;
                
                // Do target offsets
                foreach (var dash in AttackStats.TargetDashDistances.Where(d => d != -1))
                {
                    // Figure offset
                    var calc = Vector3.Add(target.WorldPosition, Vector3.Multiply(dash, direction));
                    
                    // Add to vector array
                    dashVectors[i] = new Vector3(calc.X, calc.Y, target.WorldPosition.Z);

                    // Just update every time for now i guess
                    target.WorldPosition = new Vector3(dashVectors[i].X, dashVectors[i].Y, dashVectors[i].Z);
                    
                    i++;
                }
            }

            

            // Notify users
            GameInstance.NotifyUnitWeaponSkillUsed(Owner, hits.ToList(), this, dashVectors);
        }
    }
}