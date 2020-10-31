using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Engine;
using GameServer.Configuration;
using Console = Colorful.Console;
using GameServer.Game;
using GameServer.Managers;
using GameServer.ServerPackets.Chat;
using GameServer.Web;
using Microsoft.Extensions.Configuration;
using Swan;
using Swan.Configuration;
using Swan.Logging;

namespace GameServer
{
    class Program
    {
        private static readonly AutoResetEvent WaitHandle = new AutoResetEvent(false);

        public static readonly SettingsProvider<ServerConfig> Configuration = SettingsProvider<ServerConfig>.Instance;
      
        static void Main(string[] args)
        {
            // Load config
            Configuration.ConfigurationFilePath =
                Path.Combine(Directory.GetCurrentDirectory(), "Config/server.json");

            // Write welcome banner
            WriteBanner();
            
            // Setup logging
            
            // File
            if (!Directory.Exists("Logs")) Directory.CreateDirectory("Logs");
            Logger.RegisterLogger(new FileLogger(Path.Combine(Directory.GetCurrentDirectory(), "Logs/server.log"), true)
            {
                LogLevel = LogLevel.Info
            });
            
            // Console
            ConsoleLogger.Instance.LogLevel = Configuration.Global.ConsoleLogLevel;
            
            // Startup
            "Logging started!".Info();
            
            "Reading shop data...".Info();
            
            //ShopDataReader.ReadGoods();
            
            "Done!".Info();
            
            "Reading spawn data...".Info();
            
            //SpawnDataReader.LoadAllSpawns();
            
            "Done!".Info();
            
            "Reading poo data...".Info();
            
            //PooReader.ReadPoo();
            
            "Done!".Info();
            
            "Reading map data...".Info();
            
            //GeoEngine.GeoEngine.LoadAllMaps();
            
            "Done!".Info();
            
            $"Server Port is {Configuration.Global.GamePort}".Info();
            
            "Server starting...".Info();
            
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            RunServers(token);
            
            "Server is Started!".Info();
            
            // Temp room
            
//            var instance = new GameInstance
//            {
//                //Server = sessionServer,
//                Capacity = 7,
//                GameTemplate = 10809,
//                GameType = GameType.Survival,
//                MasterId = 1,
//                GameStatus = GameStatus.InBattle,
//                GameIsStarted = true,
//                Name = "FACE OFF 24 seven",
//                SpawnLocation = new Vector3(0, 0, 80)
//            };
//            instance.LoadMap("faceonface");
//            RoomManager.CreateRoom(instance);
//            instance.StartGameLoop();
//
//            instance = new GameInstance
//            {
//                //Server = sessionServer,
//                Capacity = 8,
//                GameTemplate = 50820,
//                GameType = GameType.CaptureTheFlag,
//                MasterId = 1,
//                GameStatus = GameStatus.InBattle,
//                GameIsStarted = true,
//                Name = "VHill CTF",
//                SpawnLocation = new Vector3(1000, 0, 2000)
//            };
//            
//            instance.LoadMap("Vhill");
//            RoomManager.CreateRoom(instance);
//            instance.StartGameLoop();
//            
//            instance = new GameInstance
//            {
//                //Server = sessionServer,
//                Capacity = 100,
//                GameTemplate = 11610,
//                GameType = GameType.Survival,
//                MasterId = 1,
//                GameStatus = GameStatus.InBattle,
//                GameIsStarted = true,
//                Name = "Battle Royale?",
//                SpawnLocation = new Vector3(0, 0, 1000)
//            };
//            
//            instance.LoadMap("Tridentway");
//            RoomManager.CreateRoom(instance);
//            instance.StartGameLoop();
            
//            instance = new GameInstance
//            {
//                //Server = sessionServer,
//                Capacity = 1,
//                GameTemplate = 10800,
//                GameType = GameType.Tutorial,
//                MasterId = 1,
//                GameStatus = GameStatus.InBattle,
//                GameIsStarted = true,
//                Name = "Tutorial",
//                SpawnLocation = new Vector3(4673, 191, -3168)
//            };
            
            //instance.LoadMap("tutorial");
            //RoomManager.CreateRoom(instance);
            "Creating chat channels...".Info();
            
            // Create main chat channel
            ChatManager.CreateChannel(new ChatChannel{
                Name = "EXTEEL.NET❤️",
                OnConnect = (s, c) =>
                {
                    // This doesnt work yet. possibly sending too early?
                    c.SendMessageToSession(s, "Welcome to the Exteel.Net private server!");
                }
            });
            
            "Done!".Info();
            
            "Press Enter to stop the server or '!' to restart the server...".Info();
            
            // Docker support
            // Handle Control+C or Control+Break
            Console.CancelKeyPress += (o, e) =>
            {
                Console.WriteLine("Exit");

                tokenSource.Cancel();
                
                // Allow the manin thread to continue and exit...
                WaitHandle.Set();
            };
            
            // TESTING
            var engine = new GameEngine { TickRate = 30 };


            engine.Start();

            // Wait
            WaitHandle.WaitOne();
            
            engine.Stop();
            
            /*
            // Perform text input
            for (;;)
            {
                string line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                    break;

                // Restart the server
                if (line == "!")
                {
                    Console.Write("Server restarting...");
                    server.Restart();
                    Console.WriteLine("Done!");
                    continue;
                }
                
                // Multicast admin message to all sessions
                // TODO: Handle admin commands

                if (line[0] == '/')
                {
                    var data = Regex.Matches(line, @"[\""].+?[\""]|[^ ]+")
                        .Select(x => x.Value.Trim('"'))
                        .ToList();

                    switch (data[0])
                    {
                        case "/chat":
                            //sessionServer.Multicast(new Message(data[1], data[2], Convert.ToInt32(data[3]),
                                //Convert.ToInt32(data[4])).Write());
                            
                            server.Multicast(new Message(data[1], data[2], Convert.ToInt32(data[3]),
                                Convert.ToInt32(data[4])).Write());
                            break;
                        
                        case "/notice":
                            ChatManager.BroadcastAll("🔽🔽🔽🔽🔽🔽🔽🔽🔽🔽🔽");
                            ChatManager.BroadcastAll("🔽🔽〈🔔〉NOTICE〈🔔〉🔽🔽");
                            ChatManager.BroadcastAll("🔽🔽🔽🔽🔽🔽🔽🔽🔽🔽🔽");
                            
                            ChatManager.BroadcastAll(data[1]);
                            
                            server.Multicast(new Message("〈🔔〉NOTICE〈🔔〉", data[1]).Write());
                            break;
                    }
                }
            }*/

            // Stop the server
            "Server stopping...".Info();
            
            "Done!".Info();
            
            Terminal.Flush();
        }

        private static Task RunServers(CancellationToken token)
        {
            var webServer = AccountServer.CreateWebServer($"http://*:{Configuration.Global.WebPort}/");
            
            var webTask = webServer.RunAsync(token);
            
            var gameServer =  new GameServer(IPAddress.Any, Convert.ToInt32(Configuration.Global.GamePort));

            gameServer.OptionNoDelay = true;

            var gameTask = Task.Run(async () =>
            {
                gameServer.Start();
                while (!token.IsCancellationRequested)
                {
                    // Wait
                    await Task.Delay(10, token);
                }
                // Calling stop
                gameServer.Stop();
            }, token);

            return Task.WhenAll(webTask, gameTask);
        }
        
        #region UTIL
        
        /// <summary>
        /// Gets the assembly version
        /// </summary>
        /// <returns></returns>
        private static string GetAssemblyVersion()
        {
            return Assembly.GetEntryAssembly().GetName().Version.ToString();
        }
        
        /// <summary>
        /// Writes the banner to the console
        /// </summary>
        private static void WriteBanner()
        {
            Console.WriteAscii("Exteel.Net", Color.DarkBlue);
            
            Console.WriteLine($"Version [{GetAssemblyVersion()}]", Color.DarkBlue);
            
            Console.WriteLine($"Preparing server [{Configuration.Global.ServerName}]", Color.DarkBlue);
        }
        
        #endregion
    }
}