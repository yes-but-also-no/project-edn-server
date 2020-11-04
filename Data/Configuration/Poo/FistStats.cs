using CsvHelper.Configuration.Attributes;

namespace Data.Configuration.Poo
{
    /// <summary>
    /// The poo file data for a fist (melee) item
    /// </summary>
    public class FistStats : WeaponStatsBase
    {
        [Name("fDashDistance")]
        public float DashDistance { get; set; }
        
        [Name("fJumpAttackDashDistance")]
        public float JumpAttackDashDistance { get; set; }
        
        [Name("fBothDashDis")]
        public float BothDashDistance { get; set; }
        
        [Name("nLockOnAngle")]
        public int LockOnAngle { get; set; }
        
        [Name("nJumpAttackAngle")]
        public int JumpAttackAngle { get; set; }
        
        [Name("nRange")]
        public int Range { get; set; }
        
        [Name("nSplashCount")]
        public int SplashCount { get; set; }
        
        [Name("nSplashAngle")]
        public int SplashAngle { get; set; }
        
        [Name("nSplashDamage")]
        public int SplashDamage { get; set; }
        
        [Name("nBothDamage")]
        public int BothDamage { get; set; }
    }
    
    
}