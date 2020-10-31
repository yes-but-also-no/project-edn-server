using System.Linq;
using System.Numerics;
using Data.Model;
using GameServer.Configuration.Poo;
using GameServer.Model.Parts.Skills;
using GameServer.Model.Parts.Weapons;
using GameServer.Model.Units;
using Swan.Logging;

namespace GameServer.Game.GameModes
{
    /// <summary>
    /// Deathmatch game mode
    /// </summary>
    public class Deathmatch : GameInstance
    {
        public Deathmatch(RoomInstance roomInstance, GameStats stats) : base(roomInstance, stats)
        {
        }

        /// <summary>
        /// Users can always chat
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected override bool BeforeChat(ExteelUser user, string message)
        {
            return true;
        }

        protected override void AfterGameEnter(GameSession session)
        {
            
        }

        protected override void AfterGameLeave(GameSession session)
        {
            
        }

        protected override void AfterGameReady(GameSession session)
        {
            
        }

        /// <summary>
        /// Users can always respawn
        /// </summary>
        /// <param name="desiredUnit"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        protected override bool BeforeRegain(UnitRecord desiredUnit, GameSession owner)
        {
            return true;
        }

        protected override void AfterUnitSpawned(UserUnit unit, GameSession owner)
        {
            
        }

        protected override void AfterUnitKilled(Unit unit, Unit killer, Weapon weapon)
        {
            
        }
        
        protected override void AfterUnitKilled(Unit unit, Unit killer, Skill skill)
        {
            
        }

        protected override void BeforeGameLoad()
        {
            
        }

        protected override void BeforeGameStart()
        {
        }

        /// <summary>
        /// Each player gets a unique team
        /// </summary>
        /// <returns></returns>
        public override uint GetTeamForNewUser()
        {
            // var numSessions = RoomInstance.Sessions.Count();
            //
            // for (uint i = 0; i <= numSessions + 1; i++)
            // {
            //     if (RoomInstance.Users.All(u => u.Team != i))
            //     {
            //         $"Got team for new user ${i}".Info();
            //         return i;
            //     }
            // }
            //
            // "ERROR! Could not find a team number for new user!".Error();
            //     
            // return (uint) numSessions;
            return 0xffffffff;
        }
        
        /// <summary>
        /// The next spawn that will be used
        /// </summary>
        private int _nextSpawnId;

        public override Vector3 GetSpawnForUnit(Unit unit)
        {
            // Get spawn
            var spawn = SpawnInfo.PlayerSpawns[_nextSpawnId];
            
            // Increment
            _nextSpawnId++;

            // Loop around if needed
            if (_nextSpawnId >= SpawnInfo.PlayerSpawns.Count)
                _nextSpawnId = 0;

            return spawn.WorldPosition;
        }
    }
}