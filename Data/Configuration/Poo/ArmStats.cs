using CsvHelper.Configuration.Attributes;

namespace Data.Configuration.Poo
{
    /// <summary>
    /// The poo file data for an arm item
    /// </summary>
    public class ArmStats : PartStatsBase
    {
        /// <summary>
        /// The amount of max overheat this arm set provides
        /// </summary>
        [Name("fEndurance")]
        public float Endurance { get; set; }
        
        /// <summary>
        /// The amount of overheat recovery this arm provides when overheated
        /// </summary>
        [Name("fRecovery")]
        public float Recovery { get; set; }
    }
}