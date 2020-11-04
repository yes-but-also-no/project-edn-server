using Data.Configuration.Poo;
using Data.Model.Items;
using GameServer.Model.Units;

namespace GameServer.Model.Parts.Body
{
    /// <summary>
    /// Represents a single part in gameplay (arms, legs, etc)
    /// </summary>
    public class Part : PartBase
    {
        /// <summary>
        /// Stats for this part
        /// </summary>
        private new PartStatsBase Stats => base.Stats as PartStatsBase;
        
        /// <summary>
        /// The amount of hitpoints this part adds
        /// </summary>
        public int HitPoints => Stats.HitPoints;
        
        public Part(PartRecord partRecord, Unit owner) : base(partRecord, owner)
        {
        }
    }
}