using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Data.Model;
using Data.Model.Items;
using GameServer.Game;
using GameServer.Model.Parts.Body;
using GameServer.Model.Parts.Weapons;
using GameServer.ServerPackets.Game;
using Console = Colorful.Console;
using Weapon = GameServer.Model.Parts.Weapons.Weapon;

namespace GameServer.Model.Units
{
    /// <summary>
    /// Represents a single unit for gameplay purposes
    /// </summary>
    public abstract class Unit
    {
        /// <summary>
        /// The game instance this unit is part of
        /// </summary>
        public readonly GameInstance GameInstance;

        /// <summary>
        /// The Id of this unit
        /// </summary>
        public readonly int Id;
        
        // Part stats
        public readonly Part Head;
        public readonly Part Chest;
        public readonly Arms Arms;
        public readonly Part Legs;
        public readonly Part Booster;

        // TODO: Move into game objects
        public readonly int[] Skills = {-1, -1, -1, -1};

        protected Unit(GameInstance instance, UnitRecord unitRecord)
        {
            GameInstance = instance;
            
            Id = unitRecord.Id;
            Team = unitRecord.User?.Team ?? 0;
            Name = unitRecord.Name;

            Head = new Part(unitRecord.Head, this);
            Chest = new Part(unitRecord.Chest, this);
            Arms = new Arms(unitRecord.Arms, this);
            Legs = new Part(unitRecord.Legs, this);
            Booster = new Part(unitRecord.Backpack, this);

            var left = CreateWeapon(unitRecord.WeaponSet1Left, ArmIndex.Left, WeaponSetIndex.Primary);
            var right = unitRecord.WeaponSet1Right != null 
                ? CreateWeapon(unitRecord.WeaponSet1Right, ArmIndex.Right, WeaponSetIndex.Primary) 
                : new NullWeapon(this,  ArmIndex.Right, WeaponSetIndex.Primary);
                
            WeaponSetPrimary = (left, right);
            
            left = CreateWeapon(unitRecord.WeaponSet2Left, ArmIndex.Left, WeaponSetIndex.Secondary);
            right = unitRecord.WeaponSet2Right != null 
                ? CreateWeapon(unitRecord.WeaponSet2Right, ArmIndex.Right, WeaponSetIndex.Secondary) 
                : new NullWeapon(this, ArmIndex.Right, WeaponSetIndex.Secondary);
                
            WeaponSetSecondary = (left, right);
        }
        
        #region POSITION / MOVEMENT
        
        /// <summary>
        /// The position of this unit in the world
        /// </summary>
        public Vector3 WorldPosition;
        
        /// <summary>
        /// TODO: Remove this and use velocity or something
        /// Only used for calculating jumps atm
        /// </summary>
        public Vector3 LastWorldPosition;

        /// <summary>
        /// The Velocity of this unit
        /// </summary>
        public Vector3 Velocity;
        
        /// <summary>
        /// Is this unit in the air?
        /// </summary>
        public bool InAir;
        
        /// <summary>
        /// The y aim of this unit
        /// </summary>
        public ushort AimY;
        
        /// <summary>
        /// The x aim of this unit
        /// </summary>
        public ushort AimX;

        /// <summary>
        /// Byte flag or enum from client showing movement
        /// TODO: Find out if this is a flag or an enum
        /// </summary>
        public byte Movement = 255;
        
        /// <summary>
        /// Unknown byte or flag showing some sort of status for unit
        /// TODO: Find out if this is a flag or an enum
        /// </summary>
        public byte UnknownMovementFlag;
        
        /// <summary>
        /// Byte flag or bool from client showing booster status
        /// TODO: Find out if this is a flag or an enum
        /// </summary>
        public byte Boosting;

        /// <summary>
        /// Moves this unit according to the flags from the client
        /// </summary>
        /// <param name="flags"></param>
        public virtual void Move((byte, byte, byte) flags)
        {
            // TODO: Check for death by location in hook
            
            // Check if this is an update that needs to go out right away
            //var shouldUpdate = Movement != flags.Item1 || UnknownMovementFlag != flags.Item2 || Boosting != flags.Item3;

            Movement = flags.Item1;
            UnknownMovementFlag = flags.Item2;
            Boosting = flags.Item3;

            //if (shouldUpdate)
                GameInstance.NotifyUnitMoved(this);
        }
        
        #endregion

        #region STATS

        /// <summary>
        /// This units max health
        /// Assigned at runtime via stat calculations
        /// </summary>
        private int _maxHealth;

        /// <summary>
        /// The name of this unit
        /// </summary>
        public string Name;

        /// <summary>
        /// The session who owns this unit
        /// </summary>
        public GameSession Owner;

        /// <summary>
        /// The id of the owner of this unit
        /// </summary>
        public int UserId => Owner?.User.Id ?? -1;

        /// <summary>
        /// Calculates this units stats
        /// </summary>
        public virtual void CalculateStats()
        {
            _maxHealth = Head.HitPoints + Chest.HitPoints + Arms.HitPoints + Legs.HitPoints + Booster.HitPoints;
        }

        #endregion
        
        #region STATE

        private int _currentHealth;
        
        /// <summary>
        /// Curent health of the unit
        /// </summary>
        
        public int CurrentHealth {
            get => _currentHealth;
            set
            {
                _currentHealth = value;
                
                // Prevent overheal
                if (_currentHealth > _maxHealth)
                    _currentHealth = _maxHealth;

                if (_currentHealth <= 0)
                {
                    _currentHealth = 0;
                    GameInstance.KillUnit(this);
                }
            } 
        }

        /// <summary>
        /// Is this unit alive?
        /// </summary>
        public bool Alive => CurrentHealth > 0;

        /// <summary>
        /// The team this unit is on
        /// </summary>
        public uint Team;
        
        /// <summary>
        /// Applies damage from no source
        /// </summary>
        /// <param name="damage">Damage dealt</param>
        /// <param name="weapon">Weapon used to apply damage</param>
        public void Attack(Weapon weapon, int damage)
        {
            _currentHealth -= damage;
            
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                GameInstance.KillUnit(this, weapon.Owner, weapon);
            }
        }
        
        #endregion

        #region WEAPONS

        public (Weapon Left, Weapon Right) WeaponSetPrimary;
        public (Weapon Left, Weapon Right) WeaponSetSecondary;

        /// <summary>
        /// Is this unit using their primary weaponset
        /// </summary>
        public WeaponSetIndex WeaponSet = WeaponSetIndex.Primary;

        public (Weapon Left, Weapon Right) WeaponSetCurrent => WeaponSet == WeaponSetIndex.Primary ? WeaponSetPrimary : WeaponSetSecondary;
        
        /// <summary>
        /// Gets the weapon by arm based on the users current weapon set equipped
        /// </summary>
        /// <param name="arm">0 for left, 1 for right</param>
        /// <returns></returns>
        public Weapon GetWeaponByArm(ArmIndex arm)
        {
            return arm == ArmIndex.Left ? WeaponSetCurrent.Left : WeaponSetCurrent.Right;
        }

        /// <summary>
        /// Called when this unit aims at a unit
        /// </summary>
        /// <param name="arm"></param>
        /// <param name="targetId"></param>
        public void TryAimUnit(ArmIndex arm, int targetId)
        {
            var target = GameInstance.GetUnitById(targetId);

            if (target == null)
            {
                Console.WriteLine($"ERROR! Unit {this} tried to aim at invalid target {targetId}");
                return;
            }

            GetWeaponByArm(arm).AimUnit(target);
        }

        /// <summary>
        /// Attempst to enter sniper mode (cannons)
        /// </summary>
        /// <param name="isSniper">Is entering or exiting</param>
        public void TrySetSniperMode(bool isSniper)
        {
            // TODO: Check for any reason we cant enter
            if (isSniper)
                GameInstance.NotifySniperMode(this);
        }

        /// <summary>
        /// Attempts to switch weapons
        /// </summary>
        /// <param name="desiredWeaponSet"></param>
        public void TrySwitchWeapons(WeaponSetIndex desiredWeaponSet)
        {
            // TODO: Check for problems, like low EN, dead, etc
            
            // Set weapon set
            WeaponSet = desiredWeaponSet;
            
            // Broadcast
            GameInstance.NotifyUnitSwitchedWeapons(this);
        }

        /// <summary>
        /// Tries to perform an attack on the selected arm
        /// </summary>
        /// <param name="arm"></param>
        /// <param name="comboStep"></param>
        public void TryAttack(ArmIndex arm, int comboStep)
        {
            // Get the weapon
            var weapon = GetWeaponByArm(arm);

            // Check to make sure its real
            if (weapon is NullWeapon)
            {
                Console.WriteLine($"WARNING: {Owner} tried to call Attack on NullWeapon!", Color.Yellow);
                return;
            }

            // Try to attack
            if (!weapon.CanAttack())
            {
                return;
            }
            
            // Set combo step
            weapon.ComboStep = comboStep;

            // Do attack
            weapon.OnAttack();
            
            // Do after attack
            weapon.PostAttack();
        }

        #endregion
        
        #region SKILLS

        /// <summary>
        /// Temp until we can get the palette stuff figured out
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetSkills()
        {
            return Skills.Where(s => s != -1);
        }
        
        #endregion

        #region UTIL

        public override string ToString()
        {
            return $"[UNIT]<{Id}:{Name}>";
        }
        
        /// <summary>
        /// Creates a weapon for this unit
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        private Weapon CreateWeapon(PartRecord partRecord, ArmIndex arm, WeaponSetIndex weaponSet)
        {
            var partsType = partRecord.TemplateId;
            
            while (partsType >= 10)
                partsType /= 10;

            switch (partsType)
            {
                case 6:
                    return new Fist(partRecord, this, arm, weaponSet);
                
                case 7:
                    return new Gun(partRecord, this, arm, weaponSet);
                
                case 8:
                    // TODO: Shield stats
                    throw new NotImplementedException();
                
                default:
                    throw new Exception($"Invalid weapon template id: {partRecord.TemplateId}!");
            }
        }

        #endregion
        
        #region EVENTS
        
        /// <summary>
        /// Called when this unit is killed
        /// </summary>
        public void OnDeath()
        {
            // Clean up stuff
            WeaponSetPrimary.Left.OnDeath();
            WeaponSetPrimary.Right.OnDeath();
            
            WeaponSetSecondary.Left.OnDeath();
            WeaponSetSecondary.Right.OnDeath();

            // Remove references
            WeaponSetPrimary = (null, null);
            WeaponSetSecondary = (null, null);
        }

        /// <summary>
        /// Tick
        /// </summary>
        /// <param name="delta"></param>
        public void OnTick(double delta)
        {
            // Tick weapons
            WeaponSetPrimary.Left.OnTick(delta);
            WeaponSetPrimary.Right.OnTick(delta);
            
            WeaponSetSecondary.Left.OnTick(delta);
            WeaponSetSecondary.Right.OnTick(delta);
        }
        
        #endregion
    }

    /// <summary>
    /// Arm assignment
    /// </summary>
    public enum ArmIndex : int
    {
        Left = 0,
        Right = 1
    }

    /// <summary>
    /// Weapon set assignment
    /// </summary>
    public enum WeaponSetIndex : int
    {
        Primary = 0,
        Secondary = 1
    }
}