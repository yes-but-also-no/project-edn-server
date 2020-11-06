namespace GameServer.Web
{
    /// <summary>
    /// This is a manifest object returned to launcher clients
    /// </summary>
    public class ManifestDto
    {
        /// <summary>
        /// The servers name
        /// </summary>
        public string ServerName { get; set; }
        
        /// <summary>
        /// The servers description
        /// </summary>
        public string ServerDescription { get; set; }
        
        /// <summary>
        /// The servers game host
        /// </summary>
        public string ServerHost { get; set; }
        
        /// <summary>
        /// The servers game port
        /// </summary>
        public int ServerPort { get; set; }
        
        /// <summary>
        /// The servers web port
        /// </summary>
        public int WebPort { get; set; }
        
        /// <summary>
        /// The discord server id for the launcher
        /// </summary>
        public string DiscordServerId { get; set; }
        
        /// <summary>
        /// Max players allowed on the server
        /// </summary>
        public int MaxPlayers { get; set; }
    }

    /// <summary>
    /// This object contains server status info
    /// </summary>
    public class StatusDto
    {
        /// <summary>
        /// Server status
        /// </summary>
        public int ServerStatus { get; set; } // 0 = Online, 1 = Maintenance
        
        /// <summary>
        /// Active players
        /// </summary>
        public int PlayerCount { get; set; }
        
        /// <summary>
        /// Active room count
        /// </summary>
        public int RoomCount { get; set; }
    }
}