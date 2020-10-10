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
        /// </summary>
        public ushort Parameters { get; set; }

        /// <summary>
        /// The color of this part.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Unknown, pulled from client binary
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// To determine if it is a skill or not
        /// </summary>
        public bool IsCode => TemplateId < 100000;
    }
}