using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Model;
using GameServer.ServerPackets;
using Microsoft.EntityFrameworkCore;
using Console = Colorful.Console;

namespace GameServer.ClientPackets
{
    /// <summary>
    /// TEST
    /// </summary>
    public class ConnectWeb : ClientBasePacket
    {
        /// <summary>
        /// The user name
        /// </summary>
        private readonly Guid SessionKey;
        
        public ConnectWeb(byte[] data, GameSession client) : base(data, client)
        {
            var size = GetShort();

            var bytes = new List<byte>();
            
            for(var i=0; i< size; i++)
                bytes.Add(GetByte());
            
            SessionKey = new Guid(bytes.ToArray());
        }

        public override string GetType()
        {
            return "CONNECT_WEB";
        }

        protected override void RunImpl()
        {
            GetClient().SendPacket(TryLogin());
        }

        /// <summary>
        /// Attempts to log in and returns the correct packet
        /// </summary>
        /// <returns></returns>
        private ConnectResult TryLogin()
        {
            if (SessionKey != Guid.Parse("ea83c4eb-12b9-0248-a985-6dd0dece9e26"))
                return new ConnectResult(1);
            
            using (var db = new ExteelContext())
            {               
                // Find user
                var user = db.Users.SingleOrDefault(u => u.Username == "jake");
                
                // If not user?
                if (user == null)
                    return new ConnectResult(1);

                GetClient().UserId = user.Id;
            }
            
            GetClient().UpdateUserFromDatabase();
            
            if (string.IsNullOrEmpty(GetClient().User.Callsign))
                return new ConnectResult(-6, GetClient().User);
                
            return new ConnectResult(0, GetClient().User);
        }
    }
}