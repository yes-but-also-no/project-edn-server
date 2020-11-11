using System.Linq;
using GameServer.New;
using Network;
using Network.Packets.Client;
using Network.Packets.Client.Inventory;
using Network.Packets.Server.Inventory;

namespace GameServer.Handlers
{
    /// <summary>
    /// This class handles all packets related to bridge, stats, etc
    /// </summary>
    public static class InventoryHandler
    {
        /// <summary>
        /// Called when the client wants their inventory
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(ClientPacketType.RequestInventory)]
        public static void OnRequestInventory(GameClient client, RequestInventoryPacket packet)
        {
            var inv = client.User.Inventory;
            
            // Send money packet
            client.Send(PacketFactory.CreatePacket(new InventoryMoneyPacket
            {
                Coins = client.User.Coins,
                Credits = client.User.Credits
            }));
            
            // Send unit slots
            client.Send(PacketFactory.CreatePacket(new InventoryUnitSlotsPacket
            {
                Slots = inv.UnitSlots
            }));
            
            // Send parts
            client.Send(PacketFactory.CreatePacket(new InventoryPartsPacket
            {
                Parts = inv.Parts.Where(p => p.IsUnitPart).ToList()
            }));

            // Send units
            foreach (var unit in inv.Units)
            {
                client.Send(PacketFactory.CreatePacket(new InventoryUnitPacket
                {
                    Unit = unit,
                    UserId = client.User.Id
                }));
                
                client.Send(PacketFactory.CreatePacket(new InventoryPalettePacket
                {
                    Unit = unit
                }));
            }
            
            // Send default unit
            var defaultUnit = inv.Units.OrderBy(u => u.LaunchOrder).FirstOrDefault();
            client.Send(PacketFactory.CreatePacket(new InventoryDefaultUnitPacket
            {
                DefaultUnitId = defaultUnit?.Id ?? 0
            }));
            
            // Send special items
            client.Send(PacketFactory.CreatePacket(new InventorySpecialItemsPacket
            {
                SpecialItems = inv.Parts.Where(p => p.IsSpecialPart).ToList(),
                RepairPoints = inv.RepairPoints
            }));
            
            // Send codes
            client.Send(PacketFactory.CreatePacket(new InventoryCodeListPacket
            {
                Codes = inv.Parts.Where(p => p.IsCode).ToList()
            }));
            
            // Send operators
            client.Send(PacketFactory.CreatePacket(new InventoryOperatorListPacket
            {
                Operators = inv.Parts.Where(p => p.IsOperator).ToList()
            }));
            
            // Send end
            client.Send(PacketFactory.CreatePacket(new InventoryEndPacket
            {
                InventoryMax = (int)inv.InventorySize,
                InventoryUsed = inv.InventoryUsed
            }));
        }
        
    }
}