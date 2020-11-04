using CsvHelper.Configuration.Attributes;

namespace Data.Configuration.Poo
{
    /// <summary>
    /// Base file for all poo files
    /// </summary>
    public abstract class StatsBase
    {
        [Name("nName")]
        public uint TemplateId { get; set; }
        
//        [Name("nNPCPart")]
//        public uint NpcPart { get; set; }
        
        // TODO: Do we need other info?
        // Weight, size, etc
    }
}