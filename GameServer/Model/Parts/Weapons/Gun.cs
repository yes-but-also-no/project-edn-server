using System;
using System.Numerics;
using Data.Model.Items;
using GameServer.Configuration.Poo;
using GameServer.Model.Results;
using GameServer.Model.Units;
using GameServer.Util;
using Swan.Logging;
using Weapon = GameServer.Model.Parts.Weapons.Weapon;

namespace GameServer.Model.Parts.Weapons
{
    /// <summary>
    /// Represents a single in game gun
    /// </summary>
    public class Gun : Weapon
    {
        /// <summary>
        /// Stats for this gun
        /// </summary>
        private new GunStats Stats => base.Stats as GunStats;

        /// <summary>
        /// Projectile type
        /// </summary>
        private IfoType _ifoType => Stats.IfoType;
        
        #region STATE
        
        /// <summary>
        /// Rng for attacks
        /// </summary>
        private readonly Random _random = new Random();

        /// <summary>
        /// Used for automatic weapons
        /// </summary>
        private bool _isAttacking;

        /// <summary>
        /// Stores reload time for SMGs
        /// </summary>
        private float _currentReload;
        
        /// <summary>
        /// The target of this weapon
        /// </summary>
        public WeakReference<Unit> Target
        {
            get;
            private set;
        }
        
        #endregion
        
        public Gun(PartRecord partRecord, Unit owner, ArmIndex arm, WeaponSetIndex weaponSet) : base(partRecord, owner, arm, weaponSet)
        {
                Target = new WeakReference<Unit>(null);
        }

        #region AIMING
        
        /// <summary>
        /// Store the target here
        /// </summary>
        /// <param name="target"></param>
        public override void AimUnit(Unit target)
        {
            Target.SetTarget(target);
            
            // Broadcast
            GameInstance.NotifyUnitAimed(Owner, target, Arm);
        }
        
        /// <summary>
        /// Store the target here
        /// </summary>
        /// <param name="target"></param>
        public override void UnAimUnit()
        {
            // Save
            Target.TryGetTarget(out var oldTarget);
            
            // Clear
            Target.SetTarget(null);
            
            // Broadcast
            GameInstance.NotifyUnitUnAimed(Owner, oldTarget, Arm);
        }
        
        #endregion
        
        #region ATTACKS

        /// <summary>
        /// Called when we need to attack
        /// </summary>
        public override void OnAttack()
        {
            // Do attack
            
            // For SMG we set attacking flag true and add reload time
            if (Stats.WeaponType == WeaponType.machingun)
            {
                _isAttacking = true;
                _currentReload = Stats.ReloadTime;
            }

            // Apply overheat
            CurrentOverheat += Stats.OverheatPoint;
            
            // Check for projectile type
            if (_ifoType == IfoType.ifo_none)
            {
                var damage = 0;

                Target.TryGetTarget(out var target);
                
                // Check for valid target
                if (target != null && target.State == UnitState.InPlay)
                {
                    // Roll hit for each shot
                    for (var i = 0; i < Stats.NumberOfShots; i++)
                    {
                        var hit = _random.NextFloat();
                        damage += hit <= Stats.HitMinimum ? Stats.Damage : 0;
                        $"Rolled {hit} vs {Stats.HitMinimum:F}".Debug(source: ToString());
                    }

                    // TODO: Handle damage drop off

                    // TODO: Handle splash for non ifo lol
                }
                
                // Handle recoil
                if (Stats.RecoilDistance > 0)
                {
                    // Get their direction
                    var direction = Owner.GetAimDirection(false);

                    // Calculate end pos
                    var endPos = Owner.WorldPosition + direction * (Stats.RecoilDistance * -1);

                    // Move check
                    Owner.WorldPosition = GameInstance.Map.MoveCheck(Owner.WorldPosition, endPos);
                }

                var result = target == null
                    ? HitResult.Miss
                    : new HitResult
                    {
                        Damage = damage,
                        PushBack = Vector3.Zero,
                        ResultCode = HitResultCode.Hit,
                        VictimId = target.Id
                    };

                // Broadcast
                GameInstance.NotifyUnitAttacked(Owner, result, this);
                
                // Apply damage
                target?.Attack(this, damage);
            }
            else
            {
                Ifo ifo;

                if (Stats.IfoType == IfoType.ifo_simple)
                {
                    Target.TryGetTarget(out var target);
                    
                    // Fire IFO
                    ifo = new Ifo(this, Stats.IfoStats, GameInstance, target);
                }
                else
                {
                    ifo = new Ifo(this, Stats.IfoStats, GameInstance);
                }

                // Add to main program
                GameInstance.SpawnIfo(ifo);
                
                // Notify launched
                GameInstance.NotifyIfoLaunched(Owner, this, ifo);
            }
        }

        /// <summary>
        /// Stop attacks on overheat
        /// </summary>
        public override void PostAttack()
        {
            base.PostAttack();

            if (IsOverheated && _isAttacking)
                _isAttacking = false;
        }

        /// <summary>
        /// Called when the client stops attacking
        /// Only used for SMG
        /// </summary>
        public override void OnStopAttack()
        {
            _isAttacking = false;
            
            base.OnStopAttack();
        }

        #endregion
        
        #region EVENTS

        /// <summary>
        /// Called every frame for updates
        /// </summary>
        /// <param name="delta"></param>
        public override void OnTick(double delta)
        {
            base.OnTick(delta);

            // If we are automatic, and ready to shoot, do it
            if (_isAttacking && _currentReload <= 0)
            {
                Owner.TryAttack(Arm, 0);
            }
            else if (_currentReload > 0)
            {
                _currentReload -= Stats.ReloadTime * (float)delta / 1000;
            }
        }

        /// <summary>
        /// When our owner dies
        /// </summary>
        public override void OnDeath()
        {
            base.OnDeath();
            
            // Dealloc target
            // TODO: Send packet here?
            Target = null;

            if (_isAttacking)
                _isAttacking = false;
        }
        
        #endregion
    }
    
    public enum IfoType
    {
        ifo_none = 0,
        ifo_rocket = 1,
        ifo_missle = 2,
        ifo_sentinel_driver = 3,
        ifo_simple = 4
    }
    
    public enum WeaponType
    {
        machingun = 0,
        rifle = 1,
        cannon = 2,
        canon = 2,
        gatling = 3,
        rocket = 4,
        enggun = 5,
        shotgun = 6,
        missle = 7,
        blade = 10,
        spear = 11,
        knuckle = 12,
        spanner = 13,
        shield = 20
    }
}