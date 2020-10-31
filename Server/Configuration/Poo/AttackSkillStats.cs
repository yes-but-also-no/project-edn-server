using System;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.TypeConversion;
using GameServer.Model.Parts.Weapons;

namespace GameServer.Configuration.Poo
{
    /// <summary>
    /// The poo file data for an attack skill
    /// This is for attack skills. There is another empty file for effect based skills...
    /// </summary>
    public class AttackSkillStats
    {
        [Name("nTemplateId")]
        public uint TemplateId { get; set; }

        [Name("ECETargetTeam")]
        public TargetTeam TargetTeam { get; set; }
        
        [Name("ECETargetType")]
        [TypeConverter(typeof(TargetConverter))]
        public TargetType TargetType { get; set; }
        
        [Name("fTargetDistance")]
        [Default(0)]
        public float TargetDistance { get; set; }
        
        [Name("nDamage")]
        [Default(0)]
        public int Damage { get; set; }
        
        [Name("nHeal")]
        [Default(0)]
        public int Heal { get; set; }
        
        [Name("nSplashRadius")]
        [Default(0)]
        public int SplashRadius { get; set; }
        
        [Name("nSplashDamage")]
        [Default(0)]
        public int SplashDamage { get; set; }
        
        [Name("nMaxTarget")]
        [Default(0)]
        public int MaxTarget { get; set; }
        
        [Name("AttackerDash1")]
        public string AttackerDash1 { get; set; }
        
        [Name("AttackerDash2")]
        public string AttackerDash2 { get; set; }

        // Dash offset calculator
        public int[] AttackerDashDistances => new[]
        {
            string.IsNullOrEmpty(AttackerDash1)
                ? -1
                : Convert.ToInt32(AttackerDash1
                    .Replace("\"", "")
                    .Split(",")
                    .Last()),

            string.IsNullOrEmpty(AttackerDash2)
                ? -1
                : Convert.ToInt32(AttackerDash2
                    .Replace("\"", "")
                    .Split(",")
                    .Last())
        };
        
        [Name("TargetDash1")]
        public string TargetDash1 { get; set; }
        
        [Name("TargetDash2")]
        public string TargetDash2 { get; set; }
        
        // Dash offset calculator
        public int[] TargetDashDistances => new[]
        {
            string.IsNullOrEmpty(TargetDash1)
                ? -1
                : Convert.ToInt32(TargetDash1
                    .Replace("\"", "")
                    .Split(",")
                    .Last()),

            string.IsNullOrEmpty(TargetDash2)
                ? -1
                : Convert.ToInt32(TargetDash2
                    .Replace("\"", "")
                    .Split(",")
                    .Last())
        };
    }
    
    // Not sure this doesnt have own team...
    public enum TargetTeam
    {
        enemy = 0,
        friend = 1
    }

    public enum TargetType
    {
        near,
        activator,
        front,
        self,
        splash
    }
    
    /// <summary>
    /// Special converter for the weird backpacks
    /// </summary>
    public class TargetConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return text == "activator | splash"
                ? TargetType.activator
                : new EnumConverter(typeof(TargetType)).ConvertFromString(text, row, memberMapData);
        }
    }
}