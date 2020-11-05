using System;
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
        /// Send when the game is launched with command line arguments
        /// To launch, params must be in the format /[PARAM_NAME]:0[PARAM_DATA_BYTES]0
        /// EG: .\Exteel.exe /StartGameID:0 /SessKey:0ebc483eab9124802a9856dd0dece9e260 /RepositorySub:0 /ServerAddr:0 /MacAddr:0
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(ClientPacketType.ConnectWeb)]
        public static void OnLoginWeb(GameClient client, ConnectWebPacket packet)
        {
            // Testing
            if (packet.SessionKey == Guid.Parse("ea83c4eb-12b9-0248-a985-6dd0dece9e26"))
            {
                // Hold our result packet here
                var result = new ConnectResultPacket
                {
                    ResultCode = ConnectResultCode.Success
                };
            
                // Attempt to find user in database
                using (var db = new ExteelContext())
                {               
                    // Find user
                    var user = db.Users.SingleOrDefault(u => u.Username == "jake");
                
                    // If not user?
                    if (user == null)
                        result.ResultCode = ConnectResultCode.UserNotFound;

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
                $"Log in attempt via web. Result is {result.ResultCode}".Debug(client.ToString());
            }
        }
        
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