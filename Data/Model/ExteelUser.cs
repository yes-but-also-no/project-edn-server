
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Model.Items;

namespace Data.Model
{
    public class ExteelUser
    {
        /// <summary>
        /// Unique user Id for this user.
        /// Not sure if it is globally unique or just for the same session
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Possible remnant of social system. Not used in game so far in favor of callsign
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Username the user uses to sign into their account
        /// Not used in game so far
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The users password
        /// TODO: Switch to identity framework or encode accounts in some way
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// Callsign the user selected on first sign in. This is used as their display name
        /// </summary>
        public string Callsign { get; set; }

        /// <summary>
        /// The level of this user
        /// </summary>
        public uint Level { get; set; }

        /// <summary>
        /// The rank of this user
        /// TODO: Change to enum?
        /// </summary>
        public uint Rank { get; set; }

        /// <summary>
        /// The exp points of this user
        /// NOTE: Have not been able to see this reflected in game yet
        /// </summary>
        public uint Experience { get; set; }

        /// <summary>
        /// The amount of credits this user has
        /// </summary>
        public uint Credits { get; set; }

        /// <summary>
        /// The amount of NcCoins this user has
        /// </summary>
        public uint Coins { get; set; }

        /// <summary>
        /// The clan this user is in, if relevant
        /// </summary>
        public Clan Clan { get; set; }

        /// <summary>
        /// The pilot groth info object for this user
        /// </summary>
        public PilotInfo PilotInfo { get; set; }

        /// <summary>
        /// This user's inventory
        /// </summary>
        public UserInventory Inventory { get; set; }
        
        /// <summary>
        /// All this users stats
        /// </summary>
        public List<UserStats> Stats { get; set; }        
        
        /// <summary>
        /// Foreign key for inventory
        /// </summary>
        public int InventoryId { get; set; }
        
        /// <summary>
        /// The default unit for this user
        /// TODO: Update this when they change default unit
        /// </summary>
        public UnitRecord DefaultUnit { get; set; }
        public int DefaultUnitId { get; set; }
        
        /// <summary>
        /// The team this user is on
        /// Only used at runtime
        /// </summary>
        [NotMapped]
        public uint Team { get; set; }
        
        /// <summary>
        /// Is the user ready
        /// Only used at runtime
        /// </summary>
        [NotMapped]
        public bool IsReady { get; set; }
        
        /// <summary>
        /// The session id for this unit
        /// </summary>
        [NotMapped]
        public Guid SessionId { get; set; }
    }
}