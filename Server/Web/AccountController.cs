using System;
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
using GameServer.Handlers;
using Network;
using Network.Packets.Server.Core;
using Swan.Formatters;
using Swan.Logging;

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
            var callsign = user; // Temp
            
            // TODO: Valid no usage of stuff like GM, Admin, no brackets, etc
            if (user.ToLower().Contains("gm") || user.ToLower().Contains("admin") || user.Contains("[") ||
                user.Contains("]"))
            {
                Response.StatusCode = 400;
                return "YOUR USERNAME CONTAINS INVALID CHARACTERS";
            }

            if (string.IsNullOrEmpty(user) || user.Length < 3 || user.Length > 10)
            {
                Response.StatusCode = 400;
                return "YOUR USERNAME HAS TO BE BETWEEN 3 AND 10 CHARACTERS";
            }
            
            if (string.IsNullOrEmpty(pass) || pass.Length < 3 || pass.Length > 10)
            {
                Response.StatusCode = 400;
                return "YOUR PASSWORD HAS TO BE BETWEEN 3 AND 10 CHARACTERS";
            }

            // Now validate against db
            using (var db = new ExteelContext())
            {
                if (db.Users.Any(u => u.Username == user))
                {
                    Response.StatusCode = 400;
                    return "THAT USERNAME IS ALREADY TAKEN";
                }
                
                if (db.Users.Any(u => u.Callsign == callsign))
                {
                    Response.StatusCode = 400;
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

        [Route(HttpVerbs.Post, "/login")]
        public string Login([FormData] NameValueCollection data)
        {
            // Validate some stuff
            var username = data["username"].Trim();
            var password = data["password"].Trim();
            
            if (string.IsNullOrEmpty(username) || username.Length < 3 || username.Length > 10)
            {
                Response.StatusCode = 400;
                return "USERNAME HAS TO BE BETWEEN 3 AND 10 CHARACTERS";
            }
            
            if (string.IsNullOrEmpty(password) || password.Length < 3 || password.Length > 10)
            {
                Response.StatusCode = 400;
                return "PASSWORD HAS TO BE BETWEEN 3 AND 10 CHARACTERS";
            }
            
            // Log
            $"Log in attempt with Username {username} : {password}".Debug();
            
            // TODO: Verify not banned / locked out

            // Attempt to find user in database
            using (var db = new ExteelContext())
            {               
                // Find user
                var user = db.Users.SingleOrDefault(u => u.Username == username);
                
                // If not user?
                if (user == null)
                {
                    Response.StatusCode = 400;
                    return "USER NOT FOUND";
                }
                
                // If password wrong?
                if (user.Password != password)
                {
                    Response.StatusCode = 400;
                    return "PASSWORD INCORRECT";
                }

                // Success
                return "Success";
            }
        }
        
        [Route(HttpVerbs.Post, "/launch")]
        public LaunchDto Launch([FormData] NameValueCollection data)
        {
            // Validate some stuff
            var username = data["username"].Trim();
            var password = data["password"].Trim();
            
            if (string.IsNullOrEmpty(username) || username.Length < 3 || username.Length > 10)
            {
                Response.StatusCode = 400;
                return new LaunchDto();
            }
            
            if (string.IsNullOrEmpty(password) || password.Length < 3 || password.Length > 10)
            {
                Response.StatusCode = 400;
                return new LaunchDto();
            }
            
            // Log
            $"Log in attempt with Username {username} : {password}".Debug();
            
            // TODO: Verify not banned / locked out

            // Attempt to find user in database
            using (var db = new ExteelContext())
            {               
                // Find user
                var user = db.Users.SingleOrDefault(u => u.Username == username);
                
                // If not user?
                if (user == null)
                {
                    Response.StatusCode = 400;
                    return new LaunchDto();
                }
                
                // If password wrong?
                if (user.Password != password)
                {
                    Response.StatusCode = 400;
                    return new LaunchDto();
                }

                // Success
                return new LaunchDto
                {
                    SessionId = LoginHandler.GetNewSessionKey(user.Id).ToString().Replace("-", "")
                };
            }
        }
    }
}