using System;
using System.Drawing;
using System.Linq;
using GameServer.Managers;
using GameServer.ServerPackets;
using Console = Colorful.Console;

namespace GameServer.ClientPackets
{
    /// <summary>
    /// Sent when the client re-connects after switching to a room server
    /// </summary>
    public class ConnectSwitch : ClientBasePacket
    {
        /// <summary>
        /// The job code used to reconnect
        /// TODO: Error handling on all this
        /// </summary>
        private readonly int _jobCode;
        
        public ConnectSwitch(byte[] data, GameSession client) : base(data, client)
        {
            _jobCode = GetInt();
        }

        public override string GetType()
        {
            return "CONNECT_SWITCH";
        }

        protected override void RunImpl()
        {
            var client = GetClient();
            
           // TODO: Error handling
           ServerManager.ConnectSwitch(client, _jobCode);
           
           client.SendPacket(new ConnectResult(0, client.User));
        }
    }
}