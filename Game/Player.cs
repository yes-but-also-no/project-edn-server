using System;
using Data.Model;

namespace Game
{
    /// <summary>
    /// This represents a proxy for an actual client object
    /// It is created whenever a player joins a room
    /// It stores transient data related to gameplay for a user
    /// </summary>
    internal class Player
    {
        /// <summary>
        /// Players unique ID
        /// </summary>
        public readonly Guid Id = new Guid();

        /// <summary>
        /// Game client id
        /// </summary>
        public readonly Guid ClientId; // Maybe not needed?
        
        /// <summary>
        /// Reference to user object
        /// </summary>
        private readonly WeakReference<ExteelUser> _user;

        internal Player(ExteelUser user)
        {
            // Save ref to user
            _user = new WeakReference<ExteelUser>(user);
        }
    }
}