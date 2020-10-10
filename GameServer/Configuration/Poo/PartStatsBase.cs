using CsvHelper.Configuration.Attributes;

namespace GameServer.Configuration.Poo
{
    /// <summary>
    /// The base file for parts with HP
    /// </summary>
    public abstract class PartStatsBase : StatsBase
    {
        [Name("nHitPoint")]
        public int HitPoints { get; set; }
    }
}