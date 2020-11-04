using CsvHelper.Configuration.Attributes;

namespace Data.Configuration.Poo
{
    /// <summary>
    /// Poo file template for ability growth stats
    /// </summary>
    public class AbilityGrowth
    {
        [Name("AbilityGrowthTemplateId")]
        public uint TemplateId { get; set; }
        
        [Name("AbilityGrowthType")]
        public AbilityGrowthType AbilityGrowthType { get; set; }
        
        [Name("Level")]
        public uint Level { get; set; }
        
        [Name("CostPoint")]
        public uint CostPoint { get; set; }
        
        [Name("Effect1")]
        public uint Effect { get; set; }
    }

    public enum AbilityGrowthType
    {
        PA_UNKNOWN = 0,
        PA_HP = 1,
        PA_MV = 2,
        PA_EN = 3,
        PA_SCAN_MPU = 4,
        PA_SP_OHE = 5,
        PA_AIM_OHR = 6
    }
}