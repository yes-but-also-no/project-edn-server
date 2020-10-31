using Microsoft.Xna.Framework;

namespace Engine
{
    /// <summary>
    /// The actor represents a physical game object that has a position
    /// It also maintains a state
    /// </summary>
    public abstract class Actor : GameObject
    {
        #region PHYSICS
        
        /// <summary>
        /// Actors world location
        /// </summary>
        public Vector3 Location { get; protected set; }

        /// <summary>
        /// Actors world rotation
        /// </summary>
        public Vector3 Rotation { get; protected set; }

        /// <summary>
        /// Actors world velocity
        /// </summary>
        public Vector3 Velocity { get; protected set; }
        
        #endregion
    }
}