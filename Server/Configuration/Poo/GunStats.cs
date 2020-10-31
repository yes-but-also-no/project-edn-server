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
        
        [Name("nIFORadius")]
        public int IfoRadius { get; set; }
        
        [Name("nSplashRadius")]
        public int SplashRadius { get; set; }
        
        [Name("nSplashDamage")]
        public int SplashDamage { get; set; }
        
        [Name("nSplashCount")]
        public int SplashCount { get; set; }
        
        [Name("nPrjSpeed")]
        public int IfoSpeed { get; set; }
        
        [Name("nRange")]
        public int Range { get; set; }

        public IfoStats IfoStats => new IfoStats
        {
            Type = IfoType,
            Radius = IfoRadius,
            SplashRadius = SplashRadius,
            SplashDamage = SplashDamage,
            SplashCount = SplashCount,
            Speed = IfoSpeed,
            Range = Range,
            Damage = Damage
        };
        
        [Name("fRecoilDistance")]
        public float RecoilDistance { get; set; }
        
        [Name("fHitMin")]
        public float HitMinimum { get; set; }
    }

    public class IfoStats
    {
        public IfoType Type { get; set; }
        
        public int Radius { get; set; }
        
        public int SplashRadius { get; set; }
        
        public int SplashDamage { get; set; }
        
        public int SplashCount { get; set; }
        
        public int Speed { get; set; }
        
        public int Range { get; set; }
        
        public int Damage { get; set; }
    }
}