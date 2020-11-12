using System;
using Data.Model;

namespace Game
{
    /// <summary>
    /// This represents a proxy for an actual client object
    /// It is created whenever a player joins a room
    /// It stores transient data related to gameplay for a user
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Players unique ID
        /// </summary>
        public readonly Guid Id = new Guid();

        /// <summary>
        /// User reference
        /// </summary>
        private readonly WeakReference<ExteelUser> _user;
        
        /// <summary>
        /// Gets the user for this player
        /// </summary>
        public ExteelUser GetUser()
        {
            // Try and get
            _user.TryGetTarget(out var user);

            return user;
        }

        internal Player(ExteelUser user)
        {
            // Assign user reference
            _user = new WeakReference<ExteelUser>(user);
        }
        
        #region WAITING ROOM
        
        /// <summary>
        /// Is this player ready?
        /// </summary>
        public bool Ready { get; set; }
        
        #endregion
    }
}