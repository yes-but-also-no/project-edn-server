namespace Engine
{
    /// <summary>
    /// This represents any object that can be ticked as part of the main game loop
    /// </summary>
    public interface ITickable
    {
        /// <summary>
        /// Called to tick this object
        /// </summary>
        /// <param name="delta">time in ms since last tick</param>
        void Tick(double delta);

        /// <summary>
        /// Should this object be ticked
        /// </summary>
        bool TickEnabled { get; }

        /// <summary>
        /// The tick group for this object
        /// Defines priority
        /// </summary>
        TickGroup TickGroup { get; }
    }
    
    /// <summary>
    /// Tick priority definition
    /// </summary>
    public enum TickGroup
    {
        First,
        Network,
        Player,
        Npc,
        Last
    } 
}