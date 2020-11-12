using System;
using Core;

namespace Engine.Signals
{
    /// <summary>
    /// All game related signals
    /// </summary>
    public static class EntitySignals
    {
        /// <summary>
        /// Sent when an entity is initialized and ready for play
        /// </summary>
        public class Initialize : ASignal<Guid> {}
        
        /// <summary>
        /// Sent when an entity is spawned
        /// </summary>
        public class Spawn : ASignal<Guid> {}
        
        /// <summary>
        /// Sent when an entity is ticked
        /// </summary>
        //public class Tick : ASignal<Guid, double> {}
        
        /// <summary>
        /// Sent when an entity is de-spawned
        /// </summary>
        public class DeSpawn : ASignal<Guid> {}
        
        /// <summary>
        /// Sent when an entity is removed
        /// </summary>
        public class Remove : ASignal<Guid> {}
    }
}