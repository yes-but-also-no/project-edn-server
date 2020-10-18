using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using Cysharp.Threading;
using Data.Model;
using Data.Model.Items;
using GameServer.Configuration;
using GameServer.Configuration.Poo;
using GameServer.GeoEngine;
using GameServer.Model.Parts.Skills;
using GameServer.Model.Parts.Skills.AttackSkills;
using GameServer.Model.Parts.Weapons;
using GameServer.Model.Results;
using GameServer.Model.Units;
using GameServer.ServerPackets;
using GameServer.ServerPackets.Chat;
using GameServer.ServerPackets.Game;
using GameServer.ServerPackets.Lobby;
//using GameServer.ServerPackets.Room;
using Console = Colorful.Console;
//using UnitInfo = GameServer.ServerPackets.Room.UnitInfo;
//using UserInfo = GameServer.ServerPackets.Room.UserInfo;
using GameServer.Util;
using Swan.Configuration;
using Swan.Logging;
using Weapon = GameServer.Model.Parts.Weapons.Weapon;

// Other PACKETS
// SERVER - 
// b5 - Array of 2 ints, user id and ping time - guessing
// b4 - PING

// CLIENT -
// 58 - PONG

namespace GameServer.Game
{
    /// <summary>
    /// A single game instance
    /// Handles all data for scoring, user updates, game events, etc
    /// </summary>
    public abstract class GameInstance
    {
//        protected static readonly LogicLooper looper = new LogicLooper(Convert.ToInt32(Program.Configuration["gameFps"]));

        protected static readonly LogicLooperPool looper = 
            new LogicLooperPool(
                SettingsProvider<ServerConfig>.Instance.Global.GameFps,
                    Environment.ProcessorCount,
                    RoundRobinLogicLooperPoolBalancer.Instance
                );

        protected static readonly int UpdateFrameInterval = SettingsProvider<ServerConfig>.Instance.Global.UpdateFrameInterval;
        
        #region GAME PROPERTIES

        /// <summary>
        /// The room this game is assigned to
        /// </summary>
        public readonly RoomInstance RoomInstance;

        /// <summary>
        /// The game template stats for this game
        /// </summary>
        public readonly GameStats GameStats;
        
        /// <summary>
        /// The geo data instance for this game
        /// </summary>
        public Map Map;

        /// <summary>
        /// Creates a new game instance
        /// </summary>
        /// <param name="roomInstance">The room this game is running in</param>
        protected GameInstance(RoomInstance roomInstance, GameStats stats)
        {
            RoomInstance = roomInstance;
            
            // Look up game stats
            GameStats = stats;
        }
        
        /// <summary>
        /// Set if the game has begun
        /// This is not in packets, it is our own construct
        /// </summary>
        public GameState GameState { get; set; }

        /// <summary>
        /// Dictionary of all units in this room
        /// </summary>
        private readonly ConcurrentDictionary<int, Unit> _units = new ConcurrentDictionary<int, Unit>();
        
        /// <summary>
        /// Disctioanry of all skills in play
        /// </summary>
        private readonly ConcurrentDictionary<int, Skill> _skills = new ConcurrentDictionary<int, Skill>();

        /// <summary>
        /// Gets a skill by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Skill GetSkillById(int id)
        {
            return _skills[id];
        }

        /// <summary>
        /// Adds skills and notifies all users
        /// </summary>
        /// <param name="skill"></param>
        // public void AddSkills(IEnumerable<Skill> skills)
        // {
        //     foreach (var skill in skills)
        //     {
        //         _skills[skill.Id] = skill;
        //     }
        // }

        //TEMP
        public Vector3 SpawnLocation { get; set; } = new Vector3(0, 0, 1000);
        
        #endregion

        #region CHAT

        /// <summary>
        /// Sends a message from a user to all users in the room
        /// </summary>
        /// <param name="user">User who sent the message</param>
        /// <param name="message">The message</param>
        public virtual void OnChat(ExteelUser user, string message)
        {
            if (BeforeChat(user, message))
                RoomInstance.MulticastPacket(new Message(user.Callsign, message));
        }
        
        /// <summary>
        /// Broadcasts a message to all users in the room
        /// </summary>
        /// <param name="user">User who sent the message</param>
        /// <param name="message">The message</param>
        public virtual void BroadcastChat(string message)
        {
            RoomInstance.MulticastPacket(new Message("[SERVER]", message));
        }

        /// <summary>
        /// Called before an in game message is processed. Return false to block sending message
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns>Should send message</returns>
        protected abstract bool BeforeChat(ExteelUser user, string message);

        #endregion
        
        #region UTILITIES

        /// <summary>
        /// Gets a unit in the room by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Unit GetUnitById(int id)
        {
            // TODO: Error handling
            _units.TryGetValue(id, out var unit);

            return unit;
        }
        
        /// <summary>
        /// Returns all enemies for a unit
        /// </summary>
        /// <param name="unit">The unit to check</param>
        /// <returns></returns>
        public IEnumerable<Unit> GetEnemies(Unit unit)
        {
            return _units.Values.Where(u => u.Team != unit.Team);
        }
        
        /// <summary>
        /// Returns all team mates for a unit
        /// </summary>
        /// <param name="unit">The unit to check</param>
        /// <returns></returns>
        public IEnumerable<Unit> GetFriends(Unit unit)
        {
            return _units.Values.Where(u => u.Team == unit.Team);
        }
        
        #endregion
        
        #region JOINING / LEAVING

        /// <summary>
        /// Called when a new user joins this game. Called even if the game is not started
        /// </summary>
        /// <param name="session"></param>
        public void OnGameEnter(GameSession session)
        {
            // If in progress, list users?
            if (GameState == GameState.InGame)
            {
                // Send all users currently in room
                foreach (var user in RoomInstance.Users)
                {
                    // Send user info
                    session.SendPacket(new UserInfo(this, user));
                
                    // Send unit info
                    //session.SendPacket(new UnitInfo(user, user.DefaultUnit));
                }
            }
            
            // Call hook
            AfterGameEnter(session);
        }

        /// <summary>
        /// Called after a session enters the game
        /// </summary>
        /// <param name="session"></param>
        protected abstract void AfterGameEnter(GameSession session);

        /// <summary>
        /// Called when a user leaves the game
        /// </summary>
        /// <param name="session"></param>
        public void OnGameLeave(GameSession session)
        {
            // Broadcast quit data here
            RoomInstance.MulticastPacket(new UserLeave(session.User));
            
            // Call hook
            AfterGameLeave(session);
        }

        /// <summary>
        /// Called after a user leaves the game
        /// </summary>
        /// <param name="session"></param>
        protected abstract void AfterGameLeave(GameSession session);

        /// <summary>
        /// Called when a user has loaded into the game
        /// </summary>
        /// <param name="session"></param>
        public void OnGameReady(GameSession session)
        {
            // Set flag to true
            session.IsGameReady = true;
            
            // Load user stats here, so they are not loaded during gameplay
            //session.User.DefaultUnit.CalculateStats();
            
            // Broadcast NEW user info
            RoomInstance.MulticastPacket(new UserInfo(this, session.User));
            
            // Broadcast NEW unit info
            //MulticastPacket(new UnitInfo(session.User, session.User.DefaultUnit));
            
            // Send GEO Objects? TEST
            session.SendPacket(new GeoObjectsHp());
            
            // Spawn user default unit
            SpawnUnit(session.User.DefaultUnit, session);
            
            // If game is not started, we do start checks, otherwise, we just tell the new user its started
            if (GameState == GameState.WaitingForPlayers)
                CheckAllUsersReady();
            else
            {
//                foreach (var user in Users.Where(u => u != session.User))
//                {
//                    // Send EXISTING user info to NEW user
//                    //session.SendPacket(new ServerPackets.Game.UserInfo(this, user));
//                    
//                    
//                    
//                    // Spawn if alive
////                    if (user.DefaultUnit.Alive)
////                    {
////                        // Send EXISTING unit info to NEW user
////                        session.SendPacket(new ServerPackets.Game.UnitInfo(user, user.DefaultUnit));
////                        session.SendPacket(new SpawnUnit(user.DefaultUnit));
////                    }
//                    // Send user info
//
//                    System.Console.WriteLine($"Sending user info {user.Username} to session {session.GetUserName()}");
//                }
                // Send all active units to new session
                foreach (var unit in _units.Values)
                {
                    session.SendPacket(new UnitInfo(unit));
                    session.SendPacket(new SpawnUnit(unit));
                }
                
                session.SendPacket(new GameStarted());
            }
            
            // Call hook
            AfterGameReady(session);
        }

        protected abstract void AfterGameReady(GameSession session);
        
        /// <summary>
        /// Checks if all users are ready and begins game if they are
        /// </summary>
        private void CheckAllUsersReady()
        {
            // If everyone is loaded in
            if (RoomInstance.Sessions.All(s => s.IsGameReady))
            {
                // Start game
                OnGameStart();
            }
        }
        
        #endregion
        
        #region UNITS

        /// <summary>
        /// Tries to respawn a unit
        /// </summary>
        /// <param name="desiredUnit">The unit they wish to spawn</param>
        /// <param name="owner">Owner of the unit</param>
        /// <returns></returns>
        public bool TryRegain(UnitRecord desiredUnit, GameSession owner)
        {
            // TODO: Check for spawn timers, etc
            // TODO: Handle multiple units per user
            if (!BeforeRegain(desiredUnit, owner))
                return false;
            
            SpawnUnit(desiredUnit, owner);

            return true;
        }

        /// <summary>
        /// Called before respawning a unit. Return false to prevent spawning.
        /// </summary>
        /// <param name="desiredUnit"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        protected abstract bool BeforeRegain(UnitRecord desiredUnit, GameSession owner);

        /// <summary>
        /// Spawns a new unit into the game
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="user"></param>
        protected void SpawnUnit(UnitRecord unit, GameSession owner)
        {
            // Create the unit
            var spawnedUnit = new UserUnit(this, unit, owner);
            
            // Add to dictionary
            _units.TryAdd(unit.Id, spawnedUnit);

            // Set the current unit to the user
            owner.CurrentUnit = spawnedUnit;
            
            // Set team
            spawnedUnit.Team = owner.User.Team;

            // Calculate stats
            spawnedUnit.CalculateStats();
            
            // Set hp to max - auto clamps to max hp
            spawnedUnit.CurrentHealth = 9999;
            
            // Set unit Location
            // TODO: Spawn maps?
            spawnedUnit.WorldPosition = new Vector3(SpawnLocation.X, SpawnLocation.Y, SpawnLocation.Z);

            // Notify
            RoomInstance.MulticastPacket(new CodeList(spawnedUnit.Skills));
            
            // Send unit info
            RoomInstance.MulticastPacket(new UnitInfo(spawnedUnit));
            
            // Send spawn command
            RoomInstance.MulticastPacket(new SpawnUnit(spawnedUnit));
            
            // Send status?
            RoomInstance.MulticastPacket(new StatusChanged(spawnedUnit, true, true, true));
            
            // Call hook
            AfterUnitSpawned(spawnedUnit, owner);
        }

        /// <summary>
        /// Called after a unit is spawned
        /// </summary>
        protected abstract void AfterUnitSpawned(UserUnit unit, GameSession owner);

        /// <summary>
        /// Updates a units position
        /// Broadcasts to all users
        /// TODO: Velocity
        /// </summary>
        public void NotifyUnitMoved(Unit unit)
        {
            RoomInstance.MulticastPacket(new UnitMoved(unit, Vector3.Zero));
            
            // Fall death
            if (GameStats.FallDeath && unit.WorldPosition.Z <= GameStats.HeightFallDeath)
            {
                KillUnit(unit, unit);
            }
        }

        /// <summary>
        /// Kills a unit
        /// TODO: Find out how to handle killer / victim packets
        /// </summary>
        /// <param name="unit">The unit who died</param>
        /// <param name="killer">The unit who killed him</param>
        /// <param name="weapon">The weapon that killed him</param>
        public void KillUnit(Unit unit, Unit killer = null, Weapon weapon = null, Skill skill = null)
        {
            // Call ondeath
            unit.OnDeath();

            // Destroy?
            if (unit.Owner != null)
                unit.Owner.CurrentUnit = null;
            
            // Remove from units list
            _units.Remove(unit.Id, out var trash);

            // TODO: Use an interface here?
            if (weapon != null)
            {
                // Notify all
                RoomInstance.MulticastPacket(new UnitDestroyed(unit, killer, weapon));
            
                // Call hook
                AfterUnitKilled(unit, killer, weapon);
            }
            else
            {
                // Notify all
                RoomInstance.MulticastPacket(new UnitDestroyed(unit, killer, skill));
            
                // Call hook
                AfterUnitKilled(unit, killer, skill);
            }
            
        }

        /// <summary>
        /// Called when a unit is killed
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="killer"></param>
        /// <param name="weapon"></param>
        protected abstract void AfterUnitKilled(Unit unit, Unit killer, Weapon weapon);
        protected abstract void AfterUnitKilled(Unit unit, Unit killer, Skill skill);

        /// <summary>
        /// Updates users when a target has been made on a unit
        /// TODO: Should this be multicasted or just to the two users?
        /// TODO: Do we need to factor weapon set into this?
        /// </summary>
        /// <param name="attacker">Attacker</param>
        /// <param name="target">Target</param>
        public void NotifyUnitAimed(Unit attacker, Unit target, ArmIndex arm)
        {
            RoomInstance.MulticastPacket(new AimUnit(attacker, target, arm));     
        }

        /// <summary>
        /// Updates users when a target is no longer being aimed at
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="oldTarget"></param>
        public void NotifyUnitUnAimed(Unit attacker, Unit oldTarget, ArmIndex arm)
        {
            RoomInstance.MulticastPacket(new UnAimUnit(attacker, oldTarget, arm));
        }

        /// <summary>
        /// Updates users when a unit switches weapons
        /// </summary>
        /// <param name="unit"></param>
        public void NotifyUnitSwitchedWeapons(Unit unit)
        {
            // Broadcast
            RoomInstance.MulticastPacket(new WeaponsetChanged(unit));
        }

        /// <summary>
        ///  Notifies users when a unit enters sniper mode
        /// </summary>
        /// <param name="unit"></param>
        public void NotifySniperMode(Unit unit)
        {
            RoomInstance.MulticastPacket(new ModeSniperResult(unit));
        }

        /// <summary>
        /// Notifies users when a unit overheats a weapon
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="weapon"></param>
        public void NotifyUnitOverheatStatus(Unit unit, Weapon weapon)
        {
            RoomInstance.MulticastPacket(new OverheatStatus(unit, weapon));
        }

        /// <summary>
        /// Notifies users when an attack is made
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="victim"></param>
        /// <param name="damage"></param>
        public void NotifyUnitAttacked(Unit attacker, HitResult hit, Weapon weapon)
        {
            NotifyUnitAttacked(attacker, new List<HitResult> { hit, HitResult.Miss, HitResult.Miss, HitResult.Miss }, weapon);
        }
        
        /// <summary>
        /// Notifies users when an attack is made
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="victim"></param>
        /// <param name="damage"></param>
        public void NotifyUnitAttacked(Unit attacker, List<HitResult> hits, Weapon weapon)
        {
            while (hits.Count < 4)
                hits.Add(HitResult.Miss);
            
            if (weapon is Gun)
                RoomInstance.MulticastPacket(new Attack(attacker, hits, weapon)); 
            else if (weapon is Fist)
                RoomInstance.MulticastPacket(new AttackBlade(attacker, hits, weapon)); 
        }

        #region SKILLS

        /// <summary>
        /// Notifies users when a weapon skill is used
        /// </summary>
        /// <param name="attacker"></param>
        /// <param name="hits"></param>
        /// <param name="skill"></param>
        public void NotifyUnitWeaponSkillUsed(Unit attacker, List<HitResult> hits, WeaponSkill skill, IEnumerable<Vector3> dashVectors)
        {
            RoomInstance.MulticastPacket(new OnSkill(attacker, hits, skill, dashVectors));
        }

        #endregion

        #endregion
        
        #region NPC

        // This might get moved
        private int nextNpcId = 100000;
        
        /// <summary>
        /// Spawns an npc at a position
        /// </summary>
        /// <param name="pos">Where to spawn the npc</param>
        /// <param name="npcType">The type of npc</param>
        /// <param name="scale">How large it is</param>
        /// <param name="team">The team the npc is on</param>
        /// <returns>The unit that was created</returns>
        public Unit SpawnNpc(Vector3 pos, uint npcType = 1, float scale = 1.0f, uint team = 1000)
        {
            var unit = new UnitRecord
            {
                Id = nextNpcId++,
                Name = "NPC_Puppet_Guard",
                Head = new PartRecord
                {
                    Id = -1,
                    TemplateId = 1030603,
                    Parameters = 1,
                    Color = Color.Gray,
                    Type = 1
                },
                Chest = new PartRecord
                {
                    Id = -1,
                    TemplateId = 2030601,
                    Parameters = 1,
                    Color = Color.Gray,
                    Type = 2
                },
                Arms = new PartRecord
                {
                    Id = -1,
                    TemplateId = 3030601,
                    Parameters = 1,
                    Color = Color.Gray,
                    Type = 3
                },
                Legs = new PartRecord
                {
                    Id = -1,
                    TemplateId = 4030601,
                    Parameters = 1,
                    Color = Color.Gray,
                    Type = 4
                },
                Backpack = new PartRecord
                {
                    Id = -1,
                    TemplateId = 5030601,
                    Parameters = 1,
                    Color = Color.Gray,
                    Type = 5
                },
                WeaponSet1Left = new PartRecord
                {
                    Id = -1,
                    TemplateId = 7010101,
                    Parameters = 1,
                    Color = Color.Gray,
                    Type = 7
                },
                WeaponSet1Right = new PartRecord
                {
                    Id = -1,
                    TemplateId = 7010101,
                    Parameters = 1,
                    Color = Color.Gray,
                    Type = 7
                },
                WeaponSet2Left = new PartRecord
                {
                    Id = -1,
                    TemplateId = 7010101,
                    Parameters = 4,
                    Color = Color.Gray,
                    Type = 7
                },
                WeaponSet2Right = new PartRecord
                {
                    Id = -1,
                    TemplateId = 7010101,
                    Parameters = 1,
                    Color = Color.Gray,
                    Type = 7
                }
            };
            
            var spawnedUnit = new NpcUnit(this, unit);
            
            // Add to dictionary
            _units[unit.Id] = spawnedUnit;
            
            // Set team
            spawnedUnit.Team = team;

            // Calculate stats
            spawnedUnit.CalculateStats();
            
            // Set hp to max - auto clamps to max hp
            spawnedUnit.CurrentHealth = 9999;
            
            // Set unit Location
            // TODO: Spawn maps?
            spawnedUnit.WorldPosition = new Vector3(pos.X, pos.Y, pos.Z);
            
            // Send unit info
            RoomInstance.MulticastPacket(new NpcInfo(spawnedUnit, npcType, scale));
            
            // Send spawn command
            RoomInstance.MulticastPacket(new SpawnUnit(spawnedUnit));

            return spawnedUnit;
        }
        
        #endregion
        
        #region EVENTS

        /// <summary>
        /// Called when the game is started from the room
        /// </summary>
        public void OnGameLoad()
        {
            // Call hook
            BeforeGameLoad();
            
            // Do startup stuff
            
            // Grab ref to geo data
            LoadMap();
            
            // Start game loop
            StartGameLoop();
        }

        /// <summary>
        /// Called when all users are loaded in and gameplay begins
        /// </summary>
        public void OnGameStart()
        {
            // Call hook
            BeforeGameStart();
            
            // Set game started flag
            GameState = GameState.InGame;
                
            // Send game start
            RoomInstance.MulticastPacket(new GameStarted());
        }

        /// <summary>
        /// Called before the main game loop is initialized
        /// </summary>
        protected abstract void BeforeGameLoad();
        
        /// <summary>
        /// Called before the game starts
        /// </summary>
        protected abstract void BeforeGameStart();

        /// <summary>
        /// Called when the game is destroyed
        /// </summary>
        public void OnGameDestroy()
        {
            GameState = GameState.Destroyed;
        }
        
        #endregion
        
        #region GAME LOOP

        /// <summary>
        /// TODO: When to call this? Cancellations? etc
        /// </summary>
        private async void StartGameLoop()
        {
            await looper.RegisterActionAsync(GameLoop);
        }

        /// <summary>
        /// The main game loop. When overriding, make sure to call base function. Return false to stop game loop
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual bool GameLoop(in LogicLooperActionContext ctx)
        {
            // If game is destroyined, end loop
            if (GameState == GameState.Destroyed) return false;
            
            // If game is waiting on players, continue
            if (GameState == GameState.WaitingForPlayers) return true;
            
            // Movement
//            if (ctx.CurrentFrame % UpdateFrameInterval == 0)
//            {
//                var packets = new HashSet<ServerBasePacket>();
//                // Loop all units?
//                foreach (var unit in _units.Values)
//                {
//                    packets.Add(new UnitMoved(unit, Vector3.Zero));
//                }
//                    
//                // Send
//                RoomInstance.MulticastPacket(packets);
//            }
            
            // Tick
            foreach (var unit in _units.Values)
            {
                unit.OnTick(ctx.ElapsedTimeFromPreviousFrame.TotalMilliseconds);
            }

            return true;
        }
        
        #endregion
        
        #region GEODATA
        
        /// <summary>
        /// Assigns the geo data for this game
        /// </summary>
        /// <param name="mapName"></param>
        private void LoadMap()
        {
            Map = GeoEngine.GeoEngine.GetGeoMap(GameStats.MapFileName);
        }

        #endregion
        
        #region GAMEPLAY

        /// <summary>
        /// Gets the team for a new user
        /// If survival (deathmatch) each user is on their own team
        /// If coop, users all on team 0
        /// Otherwise, users are on either team 0 or team 1
        /// </summary>
        /// <returns></returns>
        public abstract uint GetTeamForNewUser();
        /*{
            if (GameType == GameType.Survival)
            {
                
            }

            if (GameType == GameType.DefensiveBattle)
            {
                return 0;
            }

            // Figure out which team has more users and add them to the other team
            var numUsersTeamZero = _sessions.Values.Select(s => s.User.Team).Count(t => t == 0);
            var numUsersTeamOne = _sessions.Values.Select(s => s.User.Team).Count(t => t == 1);

            // If more users on team zero, they go to team one
            // Otherwise, team zero
            return numUsersTeamZero > numUsersTeamOne ? 1 : (uint) 0;
        }*/

        
        
        #endregion
    }

    

    public enum GameStatus
    {
        Waiting = 0,
        InBattle = 1
    }

    /// <summary>
    /// Temp enum, until we can see if game status can hold this info
    /// </summary>
    public enum GameState
    {
        WaitingRoom = 0,
        WaitingForPlayers = 1,
        InGame = 2,
        GameOver = 3,
        Destroyed = 4,
    }

    /// <summary>
    /// Flags to check for collision
    /// </summary>
    public enum Collision
    {
        None = 0,
        Geo = 1,
        Units = 2
    }
}