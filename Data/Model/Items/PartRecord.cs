using System.Drawing;

namespace Data.Model.Items
{
    /// <summary>
    /// Base class for a part
    /// </summary>
    public class PartRecord
    {
        /// <summary>
        /// Unique part Id for this part.
        /// Not sure if it is globally unique or just for the same session
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Unique template Id for this part
        /// This is used to pull the part info from the poo(csv) files
        /// </summary>
        public uint TemplateId { get; set; }

        /// <summary>
        /// Unknown, pulled from client binary
        /// For now, not used in any packets, so can be safely ignored
        /// Left in for future use
        /// </summary>
        public ushort Parameters { get; set; }

        /// <summary>
        /// The color of this part.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Unknown, pulled from client binary
        /// For now, not used in any packets, so can be safely ignored
        /// Left in for future use
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// Determines if this item is a special item, which all have template id's between 1001 - 6000
        /// </summary>
        public bool IsSpecialPart => TemplateId > 1000 && TemplateId <= 6000;
        
        /// <summary>
        /// Determines if this item is an operator, which all have template id's between 
        /// </summary>
        public bool IsOperator => TemplateId > 6000 && TemplateId <= 7000;
        
        /// <summary>
        /// To determine if it is a skill or not. Skills are weird, player skills are all under ~200,
        /// but npc skills are in the 10000 - 50000 range
        /// </summary>
        public bool IsCode => TemplateId < 100000 && !IsSpecialPart && !IsOperator;

        /// <summary>
        /// Inverse of the above, all other parts are normal parts
        /// </summary>
        public bool IsUnitPart => !IsCode && !IsSpecialPart && !IsOperator;
    }
}