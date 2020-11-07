using Data.Configuration;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using GameServer.New;

namespace GameServer.Web
{
    /// <summary>
    /// This class serializes the server manifest for the launcher
    /// </summary>
    public class LauncherController : WebApiController
    {
        [Route(HttpVerbs.Get, "/manifest")]
        public ManifestDto GetManifest()
        {

            // Create the dto
            var dto = new ManifestDto
            {
                ServerName = ServerConfig.Configuration.Global.ServerName,
                ServerDescription = ServerConfig.Configuration.Global.ServerDescription,
                ServerHost = ServerConfig.Configuration.Global.GameHost,
                ServerPort = ServerConfig.Configuration.Global.GamePort,
                WebPort = ServerConfig.Configuration.Global.WebPort,
                MaxPlayers = ServerConfig.Configuration.Global.MaxPlayers,
                DiscordServerId = ServerConfig.Configuration.Global.DiscordServerId ?? ""
            };
            
            // Return to them
            return dto;
        }
        
        [Route(HttpVerbs.Get, "/status")]
        public StatusDto GetStatus()
        {

            // Create the dto
            var dto = new StatusDto
            {
                ServerStatus = 1,
                RoomCount = 0,
                PlayerCount = NewServer.Instance.ActivePlayerCount
            };
            
            // Return to them
            return dto;
        }
    }
}