using System.Linq;
using System.Text.RegularExpressions;
using GameServer.Managers;

namespace GameServer.ClientPackets.Chat
{
    /// <summary>
    /// Sent when a user sends a chat message
    /// </summary>
    public class Message : ClientBasePacket
    {
        /// <summary>
        /// The message they sent
        /// </summary>
        private readonly string _message;
        
        public Message(byte[] data, GameSession client) : base(data, client)
        {
            // Unknowns
            GetInt();
            GetInt();
            
            GetString(); // Username? - Same format in server message, but populated here, not in client message
            _message = GetString();
        }

        public override string GetType()
        {
            return "MESSAGE";
        }

        protected override void RunImpl()
        {
            var client = GetClient();
            
            // TEMP: Use user ID 1 for admin
            // Check for admin commands here
            if (_message[0] == '#' && client.User.Id == 1)
            {
                var data = Regex.Matches(_message, @"[\""].+?[\""]|[^ ]+")
                    .Select(x => x.Value.Trim('"').Trim('\0'))
                    .ToList();

                AdminManager.OnGameAdminCommand(client, data);
            }
            else
            {
                client.GameInstance?.OnChat(client.User, _message);
            }
        }
    }
}