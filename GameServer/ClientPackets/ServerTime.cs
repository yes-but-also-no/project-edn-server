using System;
using Console = Colorful.Console;

namespace GameServer.ClientPackets
{
    /// <summary>
    /// Client packet to request the server time
    /// </summary>
    public class ServerTime : ClientBasePacket
    {
        /// <summary>
        /// Time client has been connected in MS
        /// </summary>
        private readonly int _clientTime;
        
        public ServerTime(byte[] data, GameSession client) : base(data, client)
        {
            _clientTime = GetInt();
        }

        public override string GetType()
        {
            return "SERVERTIME";
        }

        protected override void RunImpl()
        {
            // Send the client their ping
            var client = GetClient();
            client.SendPacket(new ServerPackets.ServerTime(client, _clientTime));
        }
    }
}