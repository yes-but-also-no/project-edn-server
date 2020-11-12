using System;
using System.Linq;
using Network;
using Swan.Logging;
using Sylver.Network.Server;

namespace GameServer.New
{
    /// <summary>
    /// New server
    /// Singleton
    /// </summary>
    public class NewServer : NetServer<GameClient>
    {
        #region PRIVATE
        
        private const int ClientBufferSize = 128;
        private const int ClientBacklog = 50;
        
        #endregion

        /// <summary>
        /// Static instance ref
        /// </summary>
        public static NewServer Instance;
        
        /// <summary>
        /// The time stamp of when the server started
        /// </summary>
        public static DateTime StartedStamp { get; private set; }

        /// <summary>
        /// Number of active players
        /// </summary>
        public int ActivePlayerCount => Clients.Count(c => c.UserId != 0);

        /// <summary>
        /// Server Time
        /// </summary>
        public static int RunningMs => (int)(DateTime.UtcNow - StartedStamp).TotalMilliseconds;

        public NewServer(string host, int port)
        {
            PacketProcessor = new GamePacketProcessor();
            ServerConfiguration = new NetServerConfiguration(host, port, ClientBacklog, ClientBufferSize);
            
            // Assign to static instance
            Instance ??= this;

            // Mark start time
            StartedStamp = DateTime.UtcNow;
        }
        
        /// <inheritdoc />
        protected override void OnBeforeStart()
        {
            "Before server start".Info();
        }

        /// <inheritdoc />
        protected override void OnAfterStart()
        {
            "After server start".Info();
        }

        /// <inheritdoc />
        protected override void OnBeforeStop()
        {
            "Before server stop".Info();
        }

        /// <inheritdoc />
        protected override void OnClientConnected(GameClient gameClient)
        {
            $"Client connected from {gameClient.Socket.RemoteEndPoint}".Info();
        }

        /// <inheritdoc />
        protected override void OnClientDisconnected(GameClient gameClient)
        {
            $"Client disconnected from {gameClient.Socket.RemoteEndPoint}".Info();
        }
    }
}