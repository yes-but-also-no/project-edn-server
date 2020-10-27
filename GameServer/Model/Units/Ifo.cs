using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GameServer.Configuration.Poo;
using GameServer.Game;
using GameServer.Model.Parts.Weapons;
using GameServer.Model.Results;
using GameServer.Util;

namespace GameServer.Model.Units
{
    public class Ifo
    {
        /// <summary>
        /// Id of this ifo
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The position of this unit in the world
        /// </summary>
        public Vector3 WorldPosition;

        /// <summary>
        /// The direction this IFO is traveling
        /// </summary>
        public Vector3 Direction;
        
        /// <summary>
        /// How far this ifo has traveled
        /// </summary>
        private float _distanceTraveled;

        /// <summary>
        /// The weapon that fired this IFO
        /// </summary>
        public Weapon Source;

        /// <summary>
        /// Stats
        /// </summary>
        protected readonly IfoStats Stats;

        /// <summary>
        /// Calculating once to save on processing
        /// </summary>
        private readonly IEnumerable<Unit> _targets;

        /// <summary>
        /// Game instace
        /// </summary>
        private readonly GameInstance _game;

        /// <summary>
        /// For homing attacks
        /// </summary>
        private readonly Unit _target;

        public Ifo(Weapon source, IfoStats stats, GameInstance game, Unit target = null)
        {
            Source = source;
            Stats = stats;
            _game = game;
            _target = target?.State == UnitState.InPlay ? target : null;
            WorldPosition = source.Owner.GetHeadPosition();
            Direction = source.Owner.GetAimDirection();
            
            // TODO: Ifos that can hit non enemies! 
            // TODO: This wont work if new players join, but probably doesnt matter
            _targets = source.Owner.GameInstance.GetEnemies(source.Owner);
        }
        
        /// <summary>
        /// Tick
        /// </summary>
        /// <param name="delta"></param>
        public void OnTick(double delta)
        {
            // Tick ifo
            
            // Travel distance
            var distance = Stats.Speed * (float) delta / 1000;

            // Check for range max
            if (_distanceTraveled + distance > Stats.Range)
            {
                distance = Stats.Range - _distanceTraveled;
            }
            
            // Add to total
            _distanceTraveled += distance;
            
            var dir = _target != null ? Vector3.Normalize(_target.GetBodyPosition() - WorldPosition) : Direction;
            
            // Calculate new pos
            var newPos = WorldPosition + dir * distance;
            
            // Check for collisions
            var movPos = _game.Map.MoveCheck3D(WorldPosition, newPos);
            
            // Hit the map
            if (movPos != newPos)
            {
                OnHit(movPos);
                return;
            }
            
            // Move it
            WorldPosition = newPos;
            
            // If we are homing, we can hit right here
            // TODO: 50 is a random number for close enough
            if (_target != null && Vector3.Distance(WorldPosition, _target.GetBodyPosition()) < 50.0f)
            {
                OnHit(_target.GetBodyPosition(), _target);
                return;
            }
            
            // Check for unit hit
            // Find near by targets
            var hit = _targets
                .CheckInSphere(WorldPosition, Stats.Radius)
                .FirstOrDefault();

            // Attack them all
            if (hit != null)
            {
                OnHit(newPos, hit);
                return;
            }
            
            // Destroy if needed
            if (_distanceTraveled >= Stats.Range)
            {
                _game.RemoveIfo(this);
            }
        }

        public void OnHit(Vector3 position, Unit victim = null)
        {
            // Hit results
            var hits = new List<HitResult>();
            
            // If unit exits
            if (victim != null)
            {
                // Add first hit
                hits.Add(new HitResult
                {
                    VictimId = victim.Id,
                    Damage = Stats.Damage,
                    ResultCode = HitResultCode.Hit
                });
                
                // Apply damage
                victim.Attack(this.Source, Stats.Damage);
            }
            // else
            // {
            //     // Missed all
            //     hits.AddRange(new [] { HitResult.Miss, HitResult.Miss, HitResult.Miss, HitResult.Miss });
            // }

            if (Stats.SplashRadius > 0)
            {
                // Check for splash
                var splash = _targets
                        .CheckInSphere(position, Stats.SplashRadius)
                        .Where(u => u != victim)
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
                    unit.Attack(this.Source, Stats.SplashDamage);
                }
            }

            // If we still missed
            if (hits.Count == 0)
            {
                hits.AddRange(new [] { HitResult.Miss });
            }
            
            // Notify users
            _game.NotifyIfoResult(this, hits.ToList());
            
            // Remove ifo
            _game.RemoveIfo(this);
        }
    }
}