using System.Linq;
using GameServer.ServerPackets.Inventory;

namespace GameServer.ClientPackets.Inventory
{
    /// <summary>
    /// Called when a client requests their inventory
    /// </summary>
    public class RequestInventory : ClientBasePacket
    {
        public RequestInventory(byte[] data, GameSession client) : base(data, client)
        {
            HighPriority = true;
        }

        public override string GetType()
        {
            return "INV_REQ_INVENTORY";
        }

        protected override void RunImpl()
        {
            var client = GetClient();

            // Send their money
            client.SendPacket(new SendMoney(client.User));
            
            // Send unit slots
            client.SendPacket(new SendUnitSlots(client.User));
            
            // Send parts
            client.SendPacket(new SendParts(client.User));
            
            // Send units
            foreach (var unit in client.User.Inventory.Units)
            {
                // Send info
                client.SendPacket(new SendUnit(client.User, unit));
            }
            
            // Send default unit
            var defaultUnit = client.User.Inventory.Units.OrderBy(u => u.LaunchOrder).FirstOrDefault();
            client.SendPacket(new SendDefaultUnit(defaultUnit?.Id ?? 0));
            
            // Send special items
            client.SendPacket(new SendSpecialItems(client.User));
            
            // Send codes
            client.SendPacket(new SendCodeList(client.User)); 
            
            // Send palette
            client.SendPacket(new SendPalette(defaultUnit));
            
            // Send operators
            client.SendPacket(new SendOperatorList(client.User));
            
            // Send end packet
            client.SendPacket(new SendInventoryEnd(client.User));
        }
    }
}