using Swan.Logging;

namespace GameServer.Configuration
{
    /// <summary>
    /// Strong typings for all server config settings
    /// </summary>
    public class ServerConfig
    {
        public string GameHost { get; set; }
        
        public int GamePort { get; set; }
        
        public int WebPort { get; set; }
        
        public int ProtocolVersion { get; set; }
        
        public string ServerName { get; set; }
        
        public int GameFps { get; set; }
        
        public int UpdateFrameInterval { get; set; }
        
        public LogLevel ConsoleLogLevel { get; set; }
        
        public bool LogAllPackets { get; set; }
        
        public bool WriteCharacterImages { get; set; }
    }
}