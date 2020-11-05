using System.Linq;
using System.Runtime.Serialization;
using Data;
using GameServer.New;
using GameServer.ServerPackets;
using Network;
using Network.Packets.Client;
using Network.Packets.Client.Core;
using Network.Packets.Server.Core;
using Swan.Logging;

namespace GameServer.Handlers
{
    /// <summary>
    /// This class handles all login related packets
    /// </summary>
    public static class LoginHandler
    {
        /// <summary>
        /// Called when the client would like to log in
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(ClientPacketType.ConnectClient)]
        public static void OnTryLogIn(GameClient client, ConnectClientPacket packet)
        {
            // TODO: Verify not banned / locked out
            
            // Hold our result packet here
            var result = new ConnectResultPacket
            {
                ResultCode = ConnectResultCode.Success
            };
            
            // Attempt to find user in database
            using (var db = new ExteelContext())
            {               
                // Find user
                var user = db.Users.SingleOrDefault(u => u.Username == packet.UserName);
                
                // If not user?
                if (user == null)
                    result.ResultCode = ConnectResultCode.UserNotFound;
                
                // If password wrong?
                else if (user.Password != packet.Password)
                    result.ResultCode = ConnectResultCode.PasswordWrong;

                // Success
                else
                {
                    // Add to client
                    client.UserId = user.Id;
                    
                    // Pull from database
                    client.UpdateUserFromDatabase();
            
                    // Add to packet
                    result.User = client.User;
            
                    // Check for new user who has not done tutorial yet
                    if (string.IsNullOrEmpty(client.User.Callsign))
                        result.ResultCode = ConnectResultCode.SuccessNewAccount;
                }
            }

            // Send to them
            client.Send(PacketFactory.CreatePacket(result));
            
            // Log
            $"Log in attempt with Username {packet.UserName} : {packet.Password}. Result is {result.ResultCode}".Debug(client.ToString());
        }
    }
}