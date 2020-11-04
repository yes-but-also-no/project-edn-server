using CsvHelper.Configuration.Attributes;

namespace Data.Configuration.Poo
{
    /// <summary>
    /// The base class for weapon parts 
    /// </summary>
    public abstract class WeaponStatsBase : StatsBase
    {
        /// <summary>
        /// The amount of overheat this weapon generates on use
        /// </summary>
        [Name("fOverheatPoint")]
        public float OverheatPoint { get; set; }
        
        /// <summary>
        /// The amount of overheat this weapon recovers per second during normal use (not overheated)
        /// </summary>
        [Name("fOverheatRecovery")]
        public float OverheatRecovery { get; set; }
        
        /// <summary>
        /// The amount of damage this weapon does on hit
        /// </summary>
        [Name("nDamage")]
        public int Damage { get; set; }
        
        // TODO: Better enum names?
        [Name("nWeaponType")]
        public WeaponType WeaponType { get; set; } 
        
        
    }
    
    
}