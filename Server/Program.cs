#define LEGACY

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using Data.Configuration;
using Engine;
using Game;
using Console = Colorful.Console;
using GameServer.Game;
using GameServer.Managers;
using GameServer.New;
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

        private static GameServer _server;
        public static bool IsSimulating;
        
        static void Main(string[] args)
        {
            // Load config
            ServerConfig.Configuration.ConfigurationFilePath =
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
            ConsoleLogger.Instance.LogLevel = ServerConfig.Configuration.Global.ConsoleLogLevel;
            
            // Startup
            "Logging started!".Info();

            "Reading shop data...".Info();
            
            ShopDataReader.ReadGoods();
            
            "Done!".Info();
            
            #if LEGACY
            
            "Reading spawn data...".Info();
            
            SpawnDataReader.LoadAllSpawns();
            
            "Done!".Info();
            
            "Reading poo data...".Info();
            
            PooReader.ReadPoo();
            
            "Done!".Info();
            
            "Reading map data...".Info();
            /*
            GeoEngine.GeoEngine.LoadAllMaps();
            */
            #endif
            
            "Done!".Info();
            
            $"Server Port is {ServerConfig.Configuration.Global.GamePort}".Info();
            
            "Server starting...".Info();
            
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            #if LEGACY
            
            RunServersLegacy(token);
            
            #else

            RunServers(token);

            #endif
            
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
            
            //var instance = new global::Game.Game();
            

            // Wait
            WaitHandle.WaitOne();

            #if LEGACY
            
            var builder = new StringBuilder();
            
            // Perform text input
            for (;;)
            {
                string line = Console.ReadLine();

                if (line[0] == 'p')
                {
                    IsSimulating = true;
                    BeginPacketReplay(line.Remove(0, 1));
                }

                if (string.IsNullOrEmpty(line))
                {
                    Console.WriteLine("You said");
                    Console.WriteLine(builder.ToString());

                    var bytes = builder.ToString().Remove(0, 2)
                        .Split("\\x").Select(b => Byte.Parse(b, System.Globalization.NumberStyles.AllowHexSpecifier)).ToArray();
                    
                    builder.Clear();
                    
                    // Send to all
                    _server.Multicast(bytes);
                    
                    continue;
                }

                if (line.Contains("\" \\"))
                    line = line.Remove(line.Length - 3, 3);
                else
                    line = line.Remove(line.Length - 1, 1);
                
                
                builder.Append(line.Remove(0, 1));
                
                // if (string.IsNullOrEmpty(line))
                //     break;

                // Restart the server
                if (line == "!")
                {
                    Console.Write("Server restarting...");
                    
                    Console.WriteLine("Done!");
                    continue;
                }
            }

#endif

            // Stop the server
            "Server stopping...".Info();
            
            "Done!".Info();
            
            Terminal.Flush();
        }

        private static Task RunServersLegacy(CancellationToken token)
        {
            var webServer = AccountServer.CreateWebServer($"http://*:{ServerConfig.Configuration.Global.WebPort}/");
            
            var webTask = webServer.RunAsync(token);
            
            _server =  new GameServer(IPAddress.Any, Convert.ToInt32(ServerConfig.Configuration.Global.GamePort));
        
            _server.OptionNoDelay = true;
        
            var gameTask = Task.Run(async () =>
            {
                _server.Start();
                while (!token.IsCancellationRequested)
                {
                    // Wait
                    await Task.Delay(10, token);
                }
                // Calling stop
                _server.Stop();
            }, token);
        
            return Task.WhenAll(webTask, gameTask);
        }
        
        private static Task RunServers(CancellationToken token)
        {
            var webServer = AccountServer.CreateWebServer($"http://*:{ServerConfig.Configuration.Global.WebPort}/");
            
            var webTask = webServer.RunAsync(token);
            
            var gameServer =  new NewServer("0.0.0.0", Convert.ToInt32(ServerConfig.Configuration.Global.GamePort));
        
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

        private static void DoPacket(BinaryReader reader)
        {
            

            // Read packet size
            var len = reader.ReadInt16();

            // Data
            var data = new List<byte>(reader.ReadBytes(len - 2));
                    
            // Insert size
            data.InsertRange(0, BitConverter.GetBytes((short)len));
                    
            // Send
            _server.Multicast(data.ToArray());
        }
        
        private static void BeginPacketReplay(string fileName)
        {
            Console.WriteLine("Beginning packet read");
            
            
            

            var pos = 0;

            using (var csvreader = new StreamReader(Path.Combine(
                Directory.GetCurrentDirectory(), "Packets/", $"{fileName}.csv")))
            using (var csv = new CsvReader(csvreader, CultureInfo.InvariantCulture))
            using (var reader = new BinaryReader(File.Open(Path.Combine(
                Directory.GetCurrentDirectory(), "Packets/", $"{fileName}.bin"), FileMode.Open)))
            {
                csv.Configuration.Comment = ';';
                csv.Configuration.AllowComments = true;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;
                
                csv.Read();
                csv.ReadHeader();

                for (;;)
                {
                    string line = Console.ReadLine();
                    
                    // var anonymousTypeDefinition = new
                    // {
                    //     No = default(int),
                    //     Time = default(float)
                    // };
                    // var records = csv.GetRecords(anonymousTypeDefinition);

                    csv.Read();

                    var time = csv.GetField<double>("Time");

                    if (line != "play")
                    {
                        DoPacket(reader);
                        
                        // Log
                        Console.WriteLine($"Sent packet number {pos} - time {time}.");

                        // Increment
                        pos++;
                    }
                    else
                    {
                        // Log
                        Console.WriteLine("Starting playback. Have fun!");

                        while (csv.Read())
                        {
                            time = csv.GetField<double>("Time");
                            
                            DoPacket(reader);
                            
                            // Log
                            Console.WriteLine($"Sent packet number {pos} - time {time}.");

                            // Increment
                            pos++;
                            
                            // Sleep
                            Thread.Sleep((int)(1000 * time));
                        }
                    }
                }
            }
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
            
            Console.WriteLine($"Preparing server [{ServerConfig.Configuration.Global.ServerName}]", Color.DarkBlue);
        }
        
        #endregion
    }
}