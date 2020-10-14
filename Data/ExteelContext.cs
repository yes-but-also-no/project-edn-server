using System.Drawing;
using Data.Model;
using Data.Model.Items;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    /// <summary>
    /// This is the database context for all data for the server
    /// </summary>
    public class ExteelContext : DbContext
    {
        /// <summary>
        /// All users in the system
        /// </summary>
        public DbSet<ExteelUser> Users { get; set; }
        
        /// <summary>
        /// Stats for all users
        /// </summary>
        public DbSet<UserStats> UserStats { get; set; }
        
        /// <summary>
        /// All user inventories (items)
        /// </summary>
        public DbSet<UserInventory> Inventories { get; set; }
        
        /// <summary>
        /// All parts in the system
        /// </summary>
        public DbSet<PartRecord> Parts { get; set; }
        
        /// <summary>
        /// All units in the system
        /// </summary>
        public DbSet<UnitRecord> Units { get; set; }
        
        /// <summary>
        /// All clans in the system
        /// </summary>
        public DbSet<Clan> Clans { get; set; }
        
        /// <summary>
        /// Configure data base to use
        /// TODO: Support for different database types via config
        /// </summary>
        /// <param name="options"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=exteel.db");

        /// <summary>
        /// Handle custom entity mapping
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<PartRecord>()
                .Property(e => e.Color)
                .HasConversion(
                    v => v.ToArgb(),
                    v => Color.FromArgb(v));

            // Seed default users
            modelBuilder
                .Entity<ExteelUser>(u =>
                {
                    u.HasData(new ExteelUser
                    {
                        Id = 1,
                        Nickname = "Pinkett",
                        Username = "pinkett",
                        Callsign = "pinkett",
                        Coins = 10000,
                        Credits = 100000,
                        Experience = 1000,
                        Level = 50,
                        Rank = 23,
                        Password = "password",
                        InventoryId = 1,
                        //RoomId = 50,
                        DefaultUnitId = 1001
                    });

                    u.OwnsOne(e => e.PilotInfo)
                        .HasData(new PilotInfo
                        {
                            ExteelUserId = 1,
                            AbilityPointsAvailable = 2,
                            AimLevel = 5,
                            EnLevel = 1,
                            HpLevel = 0,
                            MeleeLevel = 1000,
                            MoveSpeedLevel = 0,
                            RangedLevel = 2000,
                            RocketLevel = 4000,
                            ScanRangeLevel = 0,
                            SiegeLevel = 3000,
                            SpLevel = 0
                        });
                });

            modelBuilder
                .Entity<UserStats>()
                .HasData(new UserStats
                {
                    Id = 1,
                    UserId = 1,
                    Type = Data.Model.UserStats.StatType.Training
                }, new UserStats
                {
                    Id = 2,
                    UserId = 1,
                    Type = Data.Model.UserStats.StatType.Survival
                }, new UserStats
                {
                    Id = 3,
                    UserId = 1,
                    Type = Data.Model.UserStats.StatType.TeamSurvival
                }, new UserStats
                {
                    Id = 4,
                    UserId = 1,
                    Type = Data.Model.UserStats.StatType.TeamBattle
                }, new UserStats
                {
                    Id = 5,
                    UserId = 1,
                    Type = Data.Model.UserStats.StatType.Ctf
                }, new UserStats
                {
                    Id = 6,
                    UserId = 1,
                    Type = Data.Model.UserStats.StatType.ClanBattle
                }, new UserStats
                {
                    Id = 7,
                    UserId = 1,
                    Type = Data.Model.UserStats.StatType.DefensiveBattle
                });

            // Seed default user inventory
            modelBuilder
                .Entity<UserInventory>()
                .HasData(new UserInventory
                {
                    Id = 1,
                    InventorySize = 100,
                    UnitSlots = 1
                });

            // Seed default user parts
//            modelBuilder
//                .Entity<PartRecord>()
//                .HasDiscriminator<string>("Discriminator")
//                .HasValue<PartRecord>("Part")
//                .HasValue<Weapon>("Weapon")
//                .HasValue<Code>("Code");
                
            modelBuilder
                .Entity<PartRecord>()
                .HasData(new
                {
                    UserInventoryId = 1,
                    Id = 1,
                    TemplateId = (uint)1500003,
                    Parameters = (ushort)1,
                    Color = Color.Gray,
                    Type = (byte)1
                }, new
                {
                    UserInventoryId = 1,
                    Id = 2,
                    TemplateId = (uint)2500003,
                    Parameters = (ushort)1,
                    Color = Color.Gray,
                    Type = (byte)2
                }, new
                {
                    UserInventoryId = 1,
                    Id = 3,
                    TemplateId = (uint)3500010,
                    Parameters = (ushort)1,
                    Color = Color.Gray,
                    Type = (byte)3
                }, new
                {
                    UserInventoryId = 1,
                    Id = 4,
                    TemplateId = (uint)4500003,
                    Parameters = (ushort)1,
                    Color = Color.Gray,
                    Type = (byte)4
                }, new
                {
                    UserInventoryId = 1,
                    Id = 5,
                    TemplateId = (uint)5600003,
                    Parameters = (ushort)1,
                    Color = Color.Gray,
                    Type = (byte)5
                }, new
                {
                    UserInventoryId = 1,
                    Id = 6,
                    TemplateId = (uint)7770002,
                    Parameters = (ushort)1,
                    Color = Color.Red,
                    Type = (byte)7
                }, new
                {
                    UserInventoryId = 1,
                    Id = 7,
                    TemplateId = (uint)7770001,
                    Parameters = (ushort)1,
                    Color = Color.Red,
                    Type = (byte)7,
                    Discriminator = "Weapon"
                }, new
                {
                    UserInventoryId = 1,
                    Id = 8,
                    TemplateId = (uint)6660001,
                    Parameters = (ushort)4,
                    Color = Color.Red,
                    Type = (byte)6
                }, new
                {
                    UserInventoryId = 1,
                    Id = 9,
                    TemplateId = (uint)6660002,
                    Parameters = (ushort)1,
                    Color = Color.Red,
                    Type = (byte)6
                }, new
                {
                    UserInventoryId = 1,
                    Id = 10,
                    TemplateId = (uint)6660001,
                    Parameters = (ushort)1,
                    Color = Color.Red,
                    Type = (byte)6
                }, new
                {
                    UserInventoryId = 1,
                    Id = 11,
                    TemplateId = (uint)9,
                    Parameters = (ushort)1,
                    Color = Color.Red,
                    Type = (byte)9
                });
            

            // Seed unit data
            modelBuilder
                .Entity<UnitRecord>()
                .HasData(new UnitRecord
                {
                    Id = 1001,
                    UserInventoryId = 1,
                    LaunchOrder = 0,
                    Name = "Pinky",
                    HeadId = 1,
                    ChestId = 2,
                    ArmsId = 3,
                    LegsId = 4,
                    BackpackId = 5,
                    WeaponSet1LeftId = 7,
                    WeaponSet1RightId = 8,
                    WeaponSet2LeftId = 6,
                    WeaponSet2RightId = 10
                });
           
            
        }
    }
}