using System;

namespace Engine
{
    /// <summary>
    /// Unit represents any mech (npc or player) in the game
    /// </summary>
    public class Unit : Actor, ITickable
    {
        #region INFORMATION
        
        /// <summary>
        /// The name of this unit
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Is this unit an npc
        /// </summary>
        public bool IsNpc { get; set; }
        
        /// <summary>
        /// What tick group this unit is part of
        /// </summary>
        public TickGroup TickGroup { get; private set; }
        
        #endregion
        
        #region STATE
        
        /// <summary>
        /// The current HP of this unit
        /// </summary>
        public int Health { get; private set; }
        
        /// <summary>
        /// The current EN of this unit
        /// </summary>
        public int Energy { get; private set; }
        
        /// <summary>
        /// The current SP of this unit
        /// </summary>
        public int SkillPoints { get; private set; }
        
        /// <summary>
        /// The state of this unit
        /// </summary>
        public UnitState UnitState { get; private set; }
        
        /// <summary>
        /// Should this unit tick
        /// </summary>
        public bool TickEnabled { get; private set; }

        #endregion

        #region MOVEMENT
        
        /// <summary>
        /// The current move flags of this unit
        /// TODO: Figure out what these mean
        /// </summary>
        public byte Movement { get; private set; }
        
        /// <summary>
        /// The current boost flags of this unit
        /// TODO: Figure out what these mean
        /// </summary>
        public byte Boost { get; private set; }
        
        #endregion

        #region EVENTS
        
        /// <summary>
        /// Called when the game creates this object for the first time
        /// </summary>
        public override void Initialize()
        {
            // This is no movement apparently
            Movement = 255;
            
            // Unit sate starts as out of play
            UnitState = UnitState.OutOfPlay;
            
            // Tick group depends on if its an npc or a player
            TickGroup = IsNpc ? TickGroup.Npc : TickGroup.Player;
            
            // Start out as non ticking
            TickEnabled = false;
        }

        public override void Destroy()
        {
            // Do nothing
        }

        /// <summary>
        /// Every tick
        /// </summary>
        /// <param name="delta"></param>
        public void Tick(double delta)
        {
            //throw new NotImplementedException();
        }
        
        #endregion

        #region CONTROL

        

        #endregion
        
        /// <summary>
        /// String friendly representation of unit
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[UNIT] <{Id}> {Name}";
        }
    }

    /// <summary>
    /// The state of a unit
    /// </summary>
    public enum UnitState
    {
        OutOfPlay,
        Spawned,
        InPlay,
        Dying,
        Dead
    }
}