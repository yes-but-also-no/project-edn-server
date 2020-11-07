using System.IO;
using EmbedIO;
using EmbedIO.Files;
using EmbedIO.WebApi;
using Swan.Logging;

namespace GameServer.Web
{
    /// <summary>
    /// Light weight web server for account sign ups
    /// </summary>
    public static class AccountServer
    {
        // Gets the local path of shared files.
        // When debugging, take them directly from source so we can edit and reload.
        // Otherwise, take them from the deployment directory.
        private static string HtmlRootPath
        {
            get
            {
                var assemblyPath = Path.GetDirectoryName(typeof(Program).Assembly.Location);

#if DEBUG
                return Path.Combine(Directory.GetParent(assemblyPath).Parent.Parent.FullName, "Web/html");
#else
                return Path.Combine(assemblyPath, "html");
#endif
            }
        }
        
        // Create and configure our web server.
        public static WebServer CreateWebServer(string url)
        {
            var server = new WebServer(o => o
                    .WithUrlPrefix(url)
                    .WithMode(HttpListenerMode.EmbedIO))

                .WithCors()
                
                // First, we will configure our web server by adding Modules.
                .WithWebApi("/api", m => 
                    m.WithController<AccountController>())
                .WithWebApi("/char", m => 
                    m.WithController<CharacterController>())
                .WithWebApi("/launcher", m => 
                    m.WithController<LauncherController>())
                .WithStaticFolder("/", HtmlRootPath, true, m => m
                    .WithContentCaching(true)); // Add static files after other modules to avoid conflicts
                

            // Listen for state changes.
            server.StateChanged += (s, e) => $"WebServer New State - {e.NewState}".Info();

            return server;
        }
    }
}