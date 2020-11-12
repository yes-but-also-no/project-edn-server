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

        private readonly WeakReference<ExteelUser> _user;

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