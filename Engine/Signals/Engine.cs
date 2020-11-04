using Core;

namespace Engine.Signals
{
    /// <summary>
    /// All game related signals
    /// </summary>
    public static class EngineSignals
    {
        /// <summary>
        /// Sent when the engine is initialized and ready for play
        /// </summary>
        public class Start : ASignal {}
        
        /// <summary>
        /// Sent when the engine is ticked
        /// </summary>
        public class Tick : ASignal<double> {}
        
        /// <summary>
        /// Sent when the engine is stopped
        /// </summary>
        public class Stop : ASignal {}
    }
}