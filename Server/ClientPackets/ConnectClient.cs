using System;
using System.Linq;
using Data;
using Data.Model;
using GameServer.ServerPackets;
using Microsoft.EntityFrameworkCore;
using Console = Colorful.Console;

namespace GameServer.ClientPackets
{
    /// <summary>
    /// Called when the user logs in for the first time
    /// This is patched in our client to send unencrypted info, since we could not decrypt the original packet
    /// TODO: Encrypt with different method for security?
    /// </summary>
    public class ConnectClient : ClientBasePacket
    {
        /// <summary>
        /// The user name
        /// </summary>
        private readonly string _userName;
        
        /// <summary>
        /// The password
        /// </summary>
        private readonly string _passWord;
        
        public ConnectClient(byte[] data, GameSession client) : base(data, client)
        {
            _userName = GetString().Trim();
            _passWord = GetString().Trim();
        }

        public override string GetType()
        {
            return "CONNECT_CLIENT";
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
            using (var db = new ExteelContext())
            {               
                // Find user
                var user = db.Users.SingleOrDefault(u => u.Username == _userName);
                
                // If not user?
                if (user == null)
                    return new ConnectResult(1);
                
                if (user.Password != _passWord)
                    return new ConnectResult(2);

                GetClient().UserId = user.Id;
            }
            
            GetClient().UpdateUserFromDatabase();
            
            if (string.IsNullOrEmpty(GetClient().User.Callsign))
                return new ConnectResult(-6, GetClient().User);
                
            return new ConnectResult(0, GetClient().User);
        }
    }
}