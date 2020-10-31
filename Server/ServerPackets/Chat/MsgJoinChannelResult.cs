using Data.Model;
using GameServer.Managers;

namespace GameServer.ServerPackets.Chat
{
    /// <summary>
    /// Sent as a response to joining a channel
    /// </summary>
    public class MsgJoinChannelResult : ServerBasePacket
    {
        /// <summary>
        /// Did they join successfully
        /// </summary>
        private readonly bool _success;

        /// <summary>
        /// The channel
        /// </summary>
        private readonly ChatChannel _channel;

        public MsgJoinChannelResult(bool success, ChatChannel channel = null)
        {
            _success = success;
            _channel = channel;
        }
        
        public override string GetType()
        {
            return "MSG_JOIN_CHANNEL_RESULT";
        }

        public override byte GetId()
        {
            return 0xda;
        }

        protected override void WriteImpl()
        {           
            WriteInt(_success ? 0 : 1); // Result code

            if (!_success) return;
            
            WriteInt(_channel.Id); // Channel id
            WriteString(_channel.Name); // Channel name

            var users = _channel.Users;
            
            WriteInt(users.Count); // List size
            
            // Add static server notifier
//            WriteUInt(0); // UserId
//            WriteString("〈🔔〉NOTICE〈🔔〉"); // Name
//            WriteUInt(23); // Rank
//            
//                
//            WriteByte(1); // State
//            WriteString("EVERYWHERE"); // Is this the channel name again? seems redundant
//            
//            WriteUInt(0); // ClanId
//            WriteBool(true); // Is master
//            WriteString("SERVER"); // No clan

            foreach (var user in users)
            {
                WriteInt(user.Id); // UserId
                WriteString(user.Callsign); // Name
                WriteUInt(user.Rank); // Rank
            
                
                WriteByte(1); // State - TODO: Make this work
                WriteString(_channel.Name); // Is this the channel name again? seems redundant
            
                WriteInt(user.Clan?.Id ?? 0); // ClanId
                WriteBool(false); // Is master TODO
                WriteString(user.Clan?.Name ?? ""); // No clan
            }
            
            // States:
            // 0 Offline
            // 1 Lobby [Chan Name]
            // 2 In Battle
            // 3 Individual Maintenance?
            // 4 Operator
            // 5 Shop
            // 6 Hangar
            // 7 Options
            // 8 Tutorial
            // 9 Training
            // 10+ Unknown
        }
    }
}