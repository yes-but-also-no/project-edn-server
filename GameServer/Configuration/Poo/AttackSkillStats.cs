using System;
using System.Linq;
using CsvHelper.Configuration.Attributes;
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
        public TargetType TargetType { get; set; }
        
        [Name("nDamage")]
        public int Damage { get; set; }
        
        [Name("nHeal")]
        public int Heal { get; set; }
        
        [Name("nSplashRadius")]
        public int SplashRadius { get; set; }
        
        [Name("nSplashDamage")]
        public int SplashDamage { get; set; }
        
        [Name("nMaxTarget")]
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
    }

    public enum TargetType
    {
        near,
        activator,
        front,
        self,
        splash
    }
}