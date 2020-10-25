using System;
using System.Collections.Generic;
using System.Numerics;
using GameServer.Model.Units;

namespace GameServer.Util
{
    /// <summary>
    /// Contains misc utility functions
    /// </summary>
    public static class MathUtils
    {
        public const float UnitHeadHeight = 70.0f;
        public const float UnitBodyHeight = 35.0f;

        public const float UnitHitBoxSize = 15.0f;
        
        /// <summary>
        /// Gets a normalized vector representing this units aim direction
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="includeZ"></param>
        /// <returns></returns>
        public static Vector3 GetAimDirection(this Unit unit, bool includeZ = true)
        {
            // Convert unit aim angles
            var radX = HeadingToRadians(unit.AimX);
            
            // Convert to rotation
            var rotation = Quaternion.CreateFromYawPitchRoll(0, 0, radX);
                
            // If we are ignoring the vertical aim, stop here
            //if (!includeZ)
                //return Vector3.Transform(new Vector3(1, 0, 0), rotation);
            
            // Convert unit aim angles          
            var radY = HeadingToRadians(includeZ ? unit.AimY : (ushort)0);
            
            // Apply Y aim angle 
            // Thanks @stropheum!
            rotation = rotation *
                       Quaternion.CreateFromYawPitchRoll(-radY, 0, 0);

            return Vector3.Transform(new Vector3(1, 0, 0), rotation);
        }

        
        /// <summary>
        /// Converts a heading value (unsigned short) from a packet into radians
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static float HeadingToRadians(ushort heading)
        {
            // Convert to degrees
            var deg = heading / 182.041666667f;
            
            // Convert to radians
            return deg * 0.0174533f; 
        }

        /// <summary>
        /// Gets the vector location for a units head
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static Vector3 GetHeadPosition(this Unit unit)
        {
            return new Vector3(unit.WorldPosition.X, unit.WorldPosition.Y, unit.WorldPosition.Z + UnitHeadHeight);
        }
        
        /// <summary>
        /// Gets the vector location for a units body - Half of head?
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static Vector3 GetBodyPosition(this Unit unit)
        {
            return new Vector3(unit.WorldPosition.X, unit.WorldPosition.Y, unit.WorldPosition.Z + UnitBodyHeight);
        }

        /// <summary>
        /// Finds all units in a sphere
        /// </summary>
        /// <param name="units"></param>
        /// <param name="pos"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static IEnumerable<Unit> CheckInSphere(this IEnumerable<Unit> units, Vector3 pos, float distance)
        {
            // New enumerable of units
            var foundUnits = new HashSet<Unit>();
            
            // Loop units
            foreach (var u in units)
            {
                // Calculate distance
                // TODO: Factor unit size in
                var dist = Vector3.Distance(pos, u.GetBodyPosition());

                // If they are, add to hash set
                if (dist <= (distance - UnitHitBoxSize)) foundUnits.Add(u);
            }

            return foundUnits;
        }
            
        /// <summary>
        /// Performs a cone shaped colission check for units
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="angle"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static IEnumerable<Unit> CheckInCone(this IEnumerable<Unit> units, Vector3 startPos, Vector3 direction, float angle, float length)
        {
            // Calculate base of cone radius
            var baseRadius = length * MathF.Tan(angle * 0.0174533f);

            // New enumerable of units
            var foundUnits = new HashSet<Unit>();
            
            // Loop units
            foreach (var u in units)
            {
                // Calculate distance
                // TODO: Factor unit size in
                var px = u.GetBodyPosition() - startPos;
                var coneDist = Vector3.Dot(px, direction);
                
                // Discard if outside?
                if (coneDist < 0 || coneDist > length)
                {
                    continue;
                }

                // Calculate radius of cone at distance
                var coneRadius = (coneDist / length) * baseRadius;

                // Calculate distance from center of cone
                var orthDist = (px - coneDist * direction).Length();

                // Determine if they are in the cone
                // Temp: Fake unit size by increasing radius check
                var isInCone = orthDist < coneRadius + UnitHitBoxSize;
                
                // If they are, add to hash set
                if (isInCone) foundUnits.Add(u);
            }

            return foundUnits;
        }
    }
}