using CsvHelper.Configuration.Attributes;
using GameServer.Model.Parts.Weapons;

namespace GameServer.Configuration.Poo
{
    /// <summary>
    /// The poo file data for a gun item
    /// </summary>
    public class GunStats : WeaponStatsBase
    {
        [Name("fReloadTime")]
        public float ReloadTime { get; set; }
        
        [Name("nStackNum")]
        public int NumberOfShots { get; set; }
        
        // TODO: Better enum names?
        [Name("nIFOType")]
        public IfoType IfoType { get; set; }
        
        [Name("fRecoilDistance")]
        public float RecoilDistance { get; set; }
        
        [Name("fHitMin")]
        public float HitMinimum { get; set; }
    }
    
    
}