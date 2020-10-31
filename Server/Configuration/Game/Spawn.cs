using System.Collections.Generic;
using System.Numerics;

namespace GameServer.Configuration.Game
{
    /// <summary>
    /// Represents a single spawn location
    /// </summary>
    public class Spawn
    {
        /// <summary>
        /// Id of this spawn
        /// </summary>
        public int Id { get; set; }
        
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        
        /// <summary>
        /// The position of this spawn point
        /// </summary>
        public Vector3 WorldPosition => new Vector3(X, Y, Z);
    }

    // A map of spawn info
    public class MapSpawnInfo
    {
        public Dictionary<int, Spawn> PlayerSpawns { get; set; }
    }
}