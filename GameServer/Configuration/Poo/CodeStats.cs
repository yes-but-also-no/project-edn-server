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
        
        [Name("isSuicideSkill")]
        public int IsSuicideSkill { get; set; }
        
        [Name("ECodeWeaponType")]
        public CodeEquipType CodeEquipType { get; set; }
        
        [Name("ECodeActivationTarget")]
        public CodeActivationTarget CodeActivationTarget { get; set; }
        
        [Name("fCoolTime")]
        public float CoolTime { get; set; }
        
        [Name("fTargetRange")]
        public float TargetRange { get; set; }
        
        [Name("nMaxTarget")]
        public int MaxTarget { get; set; }
        
        [Name("nProjectileCount")]
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