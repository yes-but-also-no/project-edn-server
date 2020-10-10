namespace Data.Model
{
    /// <summary>
    /// Stats for a user 
    /// </summary>
    public class UserStats
    {
        /// <summary>
        /// The id of this stats entry
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The user these stats belong to
        /// </summary>
        public ExteelUser User { get; set; }
        public int UserId { get; set; }
        
        /// <summary>
        /// The type of stat this is
        /// </summary>
        public StatType Type { get; set; }
        
        // Stats go here
        
        /// <summary>
        /// The type of stat this is
        /// Guessing: DM, TDM, TC, etc
        /// </summary>
        public enum StatType : byte
        {
            Training = 0,
            Survival = 1,
            TeamSurvival = 2,
            TeamBattle = 3,
            Ctf = 4,
            ClanBattle = 5,
            DefensiveBattle = 6
        }
    }
}