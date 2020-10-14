using System;
using System.Drawing;
using System.Linq;
using Data;
using Data.Model.Items;
using GameServer.Configuration;
using GameServer.ServerPackets.Shop;
using Microsoft.EntityFrameworkCore;
using Console = Colorful.Console;

namespace GameServer.ClientPackets.Shop
{
    /// <summary>
    /// Sent by the client when they wish to purchase an item
    /// </summary>
    public class BuyList : ClientBasePacket
    {
        // The good ID they are buying
        private readonly uint _goodId;
        
        public BuyList(byte[] data, GameSession client) : base(data, client)
        {
            // Possibly these are related to NCCoin / Credit and maybe multiple purchase options (time / durability)
            GetInt(); // Unknown, always 1
            GetInt(); // Unknown, always 1
            _goodId = GetUInt();
        }

        public override string GetType()
        {
            return "BUY_LIST";
        }

        protected override void RunImpl()
        {
            var client = GetClient();
            
            using (var db = new ExteelContext())
            {
                // Get tracked entity
                var user = db.Users
                    .Include(u => u.Inventory)
                        .ThenInclude(i => i.Parts)
                    .Single(u => u.Id == client.User.Id);
                
                Console.WriteLine("Found user " + user.Id);

                // Lookup good
                var good = ShopDataReader.GetGoodById(_goodId);

                // Subtract money
                // TODO: Handle Coins
                user.Credits -= good.CreditPrice;

                // Create Parts
                foreach (var templateId in good.Templates)
                {

                    PartRecord partRecord;

                    var partsType = (int) templateId;
                    while (partsType >= 10)
                        partsType /= 10;

                    // Check if its a code
                    if (templateId < 300)
                    {
                        partRecord = new PartRecord {Type = 9};
                    }
                    // Check for weapon
                    else if (partsType == 6 || partsType == 7 || partsType == 8)
                    {
                        partRecord = new PartRecord {Type = (byte) partsType};
                    }
                    else
                    {
                        partRecord = new PartRecord {Type = (byte) partsType};
                    }

                    // Populate info
                    
                    partRecord.TemplateId = templateId;
                    partRecord.Parameters = 1;
                    partRecord.Color = Color.White;

                    // Add to inventory
                    user.Inventory.Parts.Add(partRecord);

                    //db.Parts.Add(part);
                }

                // Save to DB
                db.SaveChanges();

            }
            
            GetClient().UpdateUserFromDatabase();
            
            // Send result
            GetClient().SendPacket(new BuyListResult(GetClient().User));
        }
    }
}