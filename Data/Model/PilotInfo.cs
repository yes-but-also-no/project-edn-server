using Microsoft.EntityFrameworkCore;

namespace Data.Model
{
    /// <summary>
    /// Contains all data regarding top level pilot progression
    /// </summary>
    [Owned]
    public class PilotInfo
    {
        public int ExteelUserId { get; set; }
        
        /// <summary>
        /// The number of points the user has available to spend
        /// </summary>
        public int AbilityPointsAvailable { get; set; }

        /// <summary>
        /// Ability progression
        /// </summary>
        public int HpLevel { get; set; }
        public int MoveSpeedLevel { get; set; }
        public int EnLevel { get; set; }
        public int ScanRangeLevel { get; set; }
        public int SpLevel { get; set; }
        public int AimLevel { get; set; }

        /// <summary>
        /// Weapon progression
        /// </summary>
        public int MeleeLevel { get; set; }
        public int RangedLevel { get; set; }
        public int SiegeLevel { get; set; }
        public int RocketLevel { get; set; }
    }
}