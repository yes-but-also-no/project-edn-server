using System;
using System.Drawing;
using System.Linq;
using GameServer.Managers;
using Console = Colorful.Console;

namespace GameServer.ClientPackets
{
    /// <summary>
    /// Sent when the user wants to switch to a room server
    /// </summary>
    public class SwitchServer : ClientBasePacket
    {
        /// <summary>
        /// Not sure how this is used yet
        /// Looks to be server id string?
        /// </summary>
        private readonly string _serverId;
        
        public SwitchServer(byte[] data, GameSession client) : base(data, client)
        {
            _serverId = GetString();
//            Console.WriteLine("Server Id? - : {0}", _serverId); // ??
//            Console.WriteLine("Int?? - : {0}", GetInt()); // ??
        }

        public override string GetType()
        {
            return "SWITCH_SERVER";
        }

        protected override void RunImpl()
        {
            var jobCode = ServerManager.JoinServer(GetClient(), _serverId);
            GetClient().SendPacket(new ServerPackets.SwitchServer(jobCode));
        }
    }
}