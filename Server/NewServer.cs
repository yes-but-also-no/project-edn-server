using Network;
using Swan.Logging;
using Sylver.Network.Server;

namespace GameServer
{
    /// <summary>
    /// Testing for new server
    /// </summary>
    public class NewServer : NetServer<GameClient>
    {
        private const int ClientBufferSize = 128;
        private const int ClientBacklog = 50;

        public NewServer(string host, int port)
        {
            PacketProcessor = new GamePacketProcessor();
            ServerConfiguration = new NetServerConfiguration(host, port, ClientBacklog, ClientBufferSize);
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