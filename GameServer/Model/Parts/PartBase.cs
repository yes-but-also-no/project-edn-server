using System.Drawing;
using Data.Model.Items;
using GameServer.Configuration;
using GameServer.Configuration.Poo;
using GameServer.Game;
using GameServer.Model.Parts.Body;
using GameServer.Model.Units;

namespace GameServer.Model.Parts
{
    /// <summary>
    /// Represents a generic part used in gameplay
    /// </summary>
    public abstract class PartBase
    {
        /// <summary>
        /// The unit that owns this part
        /// </summary>
        public readonly Unit Owner;

        /// <summary>
        /// The game instance this part is in
        /// </summary>
        protected GameInstance GameInstance => Owner.GameInstance;
        
        /// <summary>
        /// The stats for this part
        /// </summary>
        protected readonly StatsBase Stats;

        /// <summary>
        /// The color of this part
        /// </summary>
        public Color Color { get; protected set; }

        /// <summary>
        /// The id of this part
        /// </summary>
        public readonly int Id;

        public uint TemplateId => Stats.TemplateId;
        
        protected PartBase(PartRecord partRecord, Unit owner)
        {
            // For null weapon
            if (partRecord == null) return;
            
            // Load stats for part
            Stats = PooReader.GetStatsForPart(partRecord);
            
            // Set color
            Color = partRecord.Color;
            
            // Set owner
            Owner = owner;
            
            // Set the Id
            Id = partRecord.Id;

            // TODO: Any other info we need
        }
    }
}