using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using GameServer.Configuration.Poo;
using Swan.Logging;

namespace GameServer.Configuration
{
    /// <summary>
    /// This class reads game data files (.poo) into memory and helps computer unit stats
    /// </summary>
    public static class PooReader
    {
        public static List<HeadStats> Head;
        public static List<ChestStats> Chest;
        public static List<ArmStats> Arm;
        public static List<LegStats> Leg;
        public static List<BoosterStats> Booster;
        
        public static List<GunStats> Gun;
        public static List<FistStats> Fist;
        public static List<ShieldStats> Shield;

        public static List<GameStats> Game;
        
        public static List<CodeStats> Code;
        public static List<AttackSkillStats> AttackSkill;
        
        /// <summary>
        /// Reads the poo data
        /// TODO: Configure path via config file
        /// </summary>
        public static void ReadPoo()
        {
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Config/Poo/")) 
                || Directory.GetFiles("Config/Poo/", "*.csv").Length < 1)
            {
                throw new FileNotFoundException("Game data files (*.POO) are missing! Please copy them to the Config/Poo directory or run the ProjectEDN configuration tool!");
            }
            
            Head = Read<HeadStats>("head");
            Chest = Read<ChestStats>("chest");
            Arm = Read<ArmStats>("arms");
            Leg = Read<LegStats>("legs");
            Booster = Read<BoosterStats>("backpack");
            
            Gun = Read<GunStats>("weapon-gun");
            Fist = Read<FistStats>("weapon-fist");
            Shield = Read<ShieldStats>("weapon-shield");

            Game = Read<GameStats>("gametemplate");
            
            Code = Read<CodeStats>("code");
            AttackSkill = Read<AttackSkillStats>("attackskill");
        }

        /// <summary>
        /// Gets the part stats object for a template id
        /// </summary>
        /// <param name="templateId"></param>
        /// <returns></returns>
        public static StatsBase GetStatsByTemplateId(uint templateId)
        {
            var partsType = templateId;
            
            while (partsType >= 10)
                partsType /= 10;

            StatsBase partStats;

            switch (partsType)
            {
                // HEAD
                case 1:
                    partStats = Head.First(h => h.TemplateId == templateId);
                    break;
                
                // CHEST
                case 2:
                    partStats = Chest.First(h => h.TemplateId == templateId);
                    break;
                
                // ARMS
                case 3:
                    partStats = Arm.First(h => h.TemplateId == templateId);
                    break;
                
                // LEGS
                case 4:
                    partStats = Leg.First(h => h.TemplateId == templateId);
                    break;
                
                // BACKPACK / BOOSTER
                case 5:
                    partStats = Booster.First(h => h.TemplateId == templateId);
                    break;
                
                // FIST / MELEE
                case 6:
                    partStats = Fist.First(h => h.TemplateId == templateId);
                    break;
                
                // GUN
                case 7:
                    partStats = Gun.First(h => h.TemplateId == templateId);
                    break;
                
                // SHIELD
                case 8:
                    partStats = Shield.First(h => h.TemplateId == templateId);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(templateId), $"Invalid part type: {partsType}!");
            }

            return partStats;
        }

        private static List<T> Read<T>(string csvName)
        {
            List<T> data;
            
            $"Reading {csvName}(s)...".Debug();
            
            using (var reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Config/Poo/",
                $"{csvName}.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Comment = ';';
                csv.Configuration.AllowComments = true;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                
                // Read weapons
                data = csv.GetRecords<T>().ToList();
            }
            
            $"Done! Read {data.Count} {csvName}(s)!".Debug();

            return data;
        }
    }
}