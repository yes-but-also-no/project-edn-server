using Data.Model;

namespace GameServer.ServerPackets.Chat
{
    public class MsgUserJoinedChannel : ServerBasePacket
    {
        /// <summary>
        /// The user
        /// </summary>
        private readonly ExteelUser _user;

        /// <summary>
        /// The channel
        /// </summary>
        private readonly int _channelId;

        public MsgUserJoinedChannel(ExteelUser user, int channelId)
        {
            _user = user;
            _channelId = channelId;
        }
        
        public override string GetType()
        {
            return "MSG_USER_JOINED_CHANNEL";
        }

        public override byte GetId()
        {
            return 0xdb;
        }

        protected override void WriteImpl()
        {
            WriteInt(_channelId);
            
            WriteInt(_user.Id); // UserId
            WriteString(_user.Callsign); // Name
            WriteUInt(_user.Rank); // Rank
            
            WriteInt(_user.Clan?.Id ?? 0); // ClanId
            WriteBool(false); // Is master TODO
            WriteString(_user.Clan?.Name ?? ""); // No clan
        }
    }
}