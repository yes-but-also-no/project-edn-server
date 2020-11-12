using System;
using Core;

namespace Game.Signals
{
    /// <summary>
    /// All game related signals
    /// </summary>
    public static class GameSignals
    {
        /// <summary>
        /// Sent when a player joins a game
        /// </summary>
        public class PlayerJoin : ASignal<Player> {}
    }
}