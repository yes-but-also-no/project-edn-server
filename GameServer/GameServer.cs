using System;
using System.Net;
using System.Net.Sockets;
using Data;
using NetCoreServer;
using Swan.Logging;

namespace GameServer
{
    public class GameServer : TcpServer
    {
        /// <summary>
        /// The time stamp of when the client connected
        /// </summary>
        public static DateTime StartedStamp { get; set; }

        public static int RunningMs => (int)(DateTime.UtcNow - StartedStamp).TotalMilliseconds;
        
        public GameServer(IPAddress address, int port) : base(address, port)
        {
            using (var db = new ExteelContext())
            {
//                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
            
            StartedStamp = DateTime.UtcNow;
        }
        
        protected override TcpSession CreateSession()
        {
            var session = new GameSession(this);
            return session;
        }

        protected override void OnError(SocketError error)
        {
            $"Game TCP server caught an error with code {error}".Warn();
        }
    }
}