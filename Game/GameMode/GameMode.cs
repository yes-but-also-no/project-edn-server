using Core;
using Engine;

namespace Game.GameMode
{
    /// <summary>
    /// The game mode controls all functions of a game
    /// from start, to score, to finish
    /// Game modes can be created by extending this class, or by writing scripted gamemodes
    /// </summary>
    public abstract class GameMode
    {
        #region PRIVATE

        /// <summary>
        /// The game instance this game mode is being hosted in
        /// </summary>
        private readonly Game _game;

        #endregion

        #region PROTECTED

        /// <summary>
        /// Engine reference
        /// </summary>
        protected GameEngine Engine => _game.Engine;

        /// <summary>
        /// Signal hub reference
        /// </summary>
        protected SignalHub SignalHub => _game.SignalHub;

        #endregion

        
    }
}