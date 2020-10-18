using System;
using CsvHelper.Configuration.Attributes;
using GameServer.Model.Parts.Weapons;

namespace GameServer.Configuration.Poo
{
    /// <summary>
    /// The poo file data for a code item
    /// it looks like skills are split into attack and consumable,
    /// though it was never added to NA
    /// </summary>
    public class CodeStats
    {
        [Name("nTemplateId")]
        public uint TemplateId { get; set; }
        
        [BooleanTrueValues("1")]
        [BooleanFalseValues("0")]
        public bool IsSuicideSkill { get; set; }
        
        [Name("ECodeEquipType")]
        public CodeEquipType CodeEquipType { get; set; }
        
        [Name("ECodeActivationTarget")]
        public CodeActivationTarget CodeActivationTarget { get; set; }
        
        [Name("fCoolTime")]
        public float CoolTime { get; set; }
        
        [Name("fTargetRange")]
        [Default(0.0f)]
        public float TargetRange { get; set; }
        
        [Name("nMaxTarget")]
        [Default(1)]
        public int MaxTarget { get; set; }
        
        [Name("nProjectileCount")]
        [Default(1)]
        public int ProjectileCount { get; set; }
        
        [Name("AttackSkill")]
        public uint AttackSkill { get; set; }
        
        [Name("ActiveCondition")]
        public string ActiveCondition { get; set; }

        // Required SP to use
        public int RequiredSp => Convert.ToInt32(ActiveCondition.Replace("SP>=", ""));
    }
    
    public enum CodeEquipType
    {
        weapon = 0,
        backpack = 1
    }

    public enum CodeActivationTarget
    {
        self,
        enemy
    }
}