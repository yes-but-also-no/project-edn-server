using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Model;
using Data.Model.Items;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

namespace GameServer.Web
{
    public class AccountController : WebApiController
    {
        [Route(HttpVerbs.Post, "/signup")]
        public async Task<string> PostData([FormData] NameValueCollection data) 
        {
            // Validate some stuff
            var user = data["username"].Trim();
            var pass = data["password"].Trim();
            var callsign = data["callsign"].Trim();

            if (string.IsNullOrEmpty(user) || user.Length < 3 || user.Length > 10)
            {
                return "YOUR USERNAME HAS TO BE BETWEEN 3 AND 10 CHARACTERS";
            }
            
            if (string.IsNullOrEmpty(pass) || pass.Length < 3 || pass.Length > 10)
            {
                return "YOUR PASSWORD HAS TO BE BETWEEN 3 AND 10 CHARACTERS";
            }
            
            if (string.IsNullOrEmpty(callsign) || callsign.Length < 3 || callsign.Length > 10)
            {
                return "YOUR CALLSIGN HAS TO BE BETWEEN 3 AND 10 CHARACTERS";
            }
            
            // Now validate against db
            using (var db = new ExteelContext())
            {
                if (db.Users.Any(u => u.Username == user))
                {
                    return "THAT USERNAME IS ALREADY TAKEN";
                }
                
                if (db.Users.Any(u => u.Callsign == callsign))
                {
                    return "THAT CALLSIGN IS ALREADY TAKEN";
                }
                
                // If not, make them a new account
                var newUser = new ExteelUser
                {
                    Nickname = callsign,
                    Username = user,
                    Callsign = callsign,
                    Coins = 999999,
                    Credits = 10000000,
                    Experience = 0,
                    Level = 50,
                    Rank = 23,
                    Password = pass
                };
                
                newUser.PilotInfo = new PilotInfo
                {
                    AbilityPointsAvailable = 0,
                    AimLevel = 0,
                    EnLevel = 0,
                    HpLevel = 0,
                    MeleeLevel = 0,
                    MoveSpeedLevel = 0,
                    RangedLevel = 0,
                    RocketLevel = 0,
                    ScanRangeLevel = 0,
                    SiegeLevel = 0,
                    SpLevel = 0
                };
                
                newUser.Stats = new List<UserStats>
                {
                    new UserStats
                    {
                        Type = UserStats.StatType.Training
                    },
                    new UserStats
                    {
                        Type = UserStats.StatType.Survival
                    },
                    new UserStats
                    {
                        Type = UserStats.StatType.TeamSurvival
                    },
                    new UserStats
                    {
                        Type = UserStats.StatType.TeamBattle
                    },
                    new UserStats
                    {
                        Type = UserStats.StatType.Ctf
                    },
                    new UserStats
                    {
                        Type = UserStats.StatType.ClanBattle
                    },
                    new UserStats
                    {
                        Type = UserStats.StatType.DefensiveBattle
                    }
                };

                newUser.Inventory = new UserInventory
                {
                    InventorySize = 100,
                    UnitSlots = 1,
                    RepairPoints = 100
                };
                
                newUser.Inventory.Parts = new List<PartRecord>
                {
                    new PartRecord
                    {
                        TemplateId = 1110001,
                        Parameters = 1,
                        Color = Color.Gray,
                        Type = 1
                    },
                    new PartRecord
                    {
                        TemplateId = 2220001,
                        Parameters = 1,
                        Color = Color.Gray,
                        Type = 2
                    },
                    new PartRecord
                    {
                        TemplateId = 3330001,
                        Parameters = 1,
                        Color = Color.Gray,
                        Type = 3
                    },
                    new PartRecord
                    {
                        TemplateId = 4440001,
                        Parameters = 1,
                        Color = Color.Gray,
                        Type = 4
                    },
                    new PartRecord
                    {
                        TemplateId = 5550001,
                        Parameters = 1,
                        Color = Color.Gray,
                        Type = 5
                    },
                    new PartRecord
                    {
                        TemplateId = 7770016,
                        Parameters = 1,
                        Color = Color.Gray,
                        Type = 7
                    },
                    new PartRecord
                    {
                        TemplateId = 7770016,
                        Parameters = 1,
                        Color = Color.Gray,
                        Type = 7
                    },
                    new PartRecord
                    {
                        TemplateId = 7770016,
                        Parameters = 4,
                        Color = Color.Gray,
                        Type = 7
                    },
                    new PartRecord
                    {
                        TemplateId = 7770016,
                        Parameters = 1,
                        Color = Color.Gray,
                        Type = 7
                    }, new PartRecord
                    {
                        TemplateId = 1002,
                        Parameters = 1,
                        Color = Color.White,
                        Type = 9
                    }, new PartRecord
                    {
                        TemplateId = 6001,
                    }
                };
                
                newUser.Inventory.Units = new List<UnitRecord>
                {
                    new UnitRecord
                    {
                        LaunchOrder = 0,
                        Name = "ExteelLives",
                        Head = newUser.Inventory.Parts[0],
                        Chest = newUser.Inventory.Parts[1],
                        Arms = newUser.Inventory.Parts[2],
                        Legs = newUser.Inventory.Parts[3],
                        Backpack = newUser.Inventory.Parts[4],
                        WeaponSet1Left = newUser.Inventory.Parts[5],
                        WeaponSet1Right = newUser.Inventory.Parts[6],
                        WeaponSet2Left = newUser.Inventory.Parts[7],
                        WeaponSet2Right = newUser.Inventory.Parts[8]
                    }
                };

                newUser.Operator = newUser.Inventory.Parts[10];
                newUser.DefaultUnit = newUser.Inventory.Units[0];

                db.Users.Add(newUser);

                db.SaveChanges();
            }

            return "OK YOURE SIGNED UP";
        }
    }
}