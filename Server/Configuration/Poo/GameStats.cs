using CsvHelper.Configuration.Attributes;

namespace GameServer.Configuration.Poo
{
    /// <summary>
    /// Poo file data for game templates
    /// </summary>
    public class GameStats
    {
        /// <summary>
        /// The template id for this game
        /// </summary>
        [Name("nTemplateId")]
        public uint TemplateId { get; set; }
        
        /// <summary>
        /// The template group for this game template (first three digits) for handling random
        /// </summary>
        public string TemplateGroup => TemplateId.ToString().Substring(0, 3); 
            
        [Name("MapFileName")]
        public string MapFileName { get; set; }
        
        [Name("GameType")]
        public GameType GameType { get; set; }
        
        /// <summary>
        /// Max players that can be in the game
        /// </summary>
        [Name("MaxPlayers")]
        public int MaxPlayers { get; set; }
        
        /// <summary>
        /// Number of players needed to start the game
        /// </summary>
        [Name("StartPlayers")]
        public int StartPlayers { get; set; }
        
        /// <summary>
        /// Match length in minutes?
        /// There is a playtime2, not sure purpose. Maybe for last stand only?
        /// </summary>
        [Name("PlayTime")]
        public int PlayTime { get; set; }
        
        /// <summary>
        /// Is fall death enabled?
        /// </summary>
        [Name("FallDeath")]
        public bool FallDeath { get; set; }
        
        /// <summary>
        /// Height at which players die from falling
        /// </summary>
        [Name("cfHeightFallDeath")]
        public float HeightFallDeath { get; set; }
    }
    
    public enum GameType
    {
        training = 0,
        survival = 1,
        teamsurvival = 2,
        teambattle = 3,
        clanbattle = 4,
        defensivebattle = 5,
        tutorial = 6,
        freemode = 7,
        capturetheflag = 8
    }
}