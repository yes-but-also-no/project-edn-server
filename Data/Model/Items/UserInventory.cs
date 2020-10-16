using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Data.Model.Items
{
    /// <summary>
    /// A users inventory
    /// </summary>
    public class UserInventory
    {
        /// <summary>
        /// Unique Id for this inventory.
        /// Just used for database management
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The use who owns this inventory
        /// </summary>
        public ExteelUser User { get; set; }
        
        /// <summary>
        /// This user's units
        /// </summary>
        public List<UnitRecord> Units { get; set; }

        /// <summary>
        /// This users parts
        /// </summary>
        public List<PartRecord> Parts { get; set; }

        /// <summary>
        /// Number of unit slots this user has
        /// </summary>
        public uint UnitSlots { get; set; }

        /// <summary>
        /// The maximum size of this users inventory
        /// </summary>
        public uint InventorySize { get; set; }
        
        /// <summary>
        /// The amount of repair points this user has
        /// </summary>
        public uint RepairPoints { get; set; }

        /// <summary>
        /// The current number of items in the users inventory
        /// </summary>
        public int InventoryUsed => Parts.Count;
    }
}