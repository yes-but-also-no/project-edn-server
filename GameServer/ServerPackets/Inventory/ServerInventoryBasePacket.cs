using System.Linq;
using Data.Model;
using Data.Model.Items;
using Microsoft.EntityFrameworkCore;

namespace GameServer.ServerPackets.Inventory
{
    /// <summary>
    /// Base packet for all inventory packets
    /// </summary>
    public abstract class ServerInventoryBasePacket : ServerBasePacket
    {
        /// <summary>
        /// The inventory for the user
        /// </summary>
        protected readonly UserInventory Inventory;
        
        public ServerInventoryBasePacket(ExteelUser user)
        {
            // Load the users inventory
            Inventory = user.Inventory;
        }
    }
}