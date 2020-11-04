using System;
using System.Drawing;
using Data.Configuration;
using GameServer.ServerPackets;
using Swan.Configuration;
using Console = Colorful.Console;

namespace GameServer.ClientPackets
{
    /// <summary>
    /// Packet when user reports their protocol version
    /// </summary>
    public class ProtocolVersion : ClientBasePacket
    {
        /// <summary>
        /// The version of protocol the client uses
        /// Currently is 333
        /// </summary>
        private readonly int _version;
        
        public ProtocolVersion(byte[] data, GameSession client) : base(data, client)
        {
            try
            {
                _version = GetInt();
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error on client protocol version!");
                _version = 0;
                client.Disconnect();
            }
        }

        protected override void RunImpl()
        {
            // TODO: Check version?
            
            // Set on client
            GetClient().Version = _version;
            
            // TODO: If debug
            Console.WriteLine("Client protocol is : {0}", _version, Color.DodgerBlue);

            if (_version != SettingsProvider<ServerConfig>.Instance.Global.ProtocolVersion)
            {
                // TODO: Respond with invalid version?
                System.Console.WriteLine("Client has invalid version!");
                GetClient().Disconnect();
                return;
            }
            
            // Send the client their version
            GetClient().SendPacket(new ServerVersion());
        }
        
        public override string GetType()
        {
            return "VERSION";
        }
    }
}