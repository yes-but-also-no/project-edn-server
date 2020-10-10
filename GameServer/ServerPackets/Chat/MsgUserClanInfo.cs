using Data.Model;

namespace GameServer.ServerPackets.Chat
{
    /// <summary>
    /// Sent by server to give clan info for a user?
    /// </summary>
    public class MsgUserClanInfo : ServerBasePacket
    {
        /// <summary>
        /// The user who this packet pertains to
        /// </summary>
        private readonly ExteelUser _user;

        public MsgUserClanInfo(ExteelUser user)
        {
            _user = user;
        }

        public override string GetType()
        {
            return "MSG_USER_CLAN_INFO";
        }

        public override byte GetId()
        {
            return 0xd3;
        }

        protected override void WriteImpl()
        {           
            WriteInt(_user.Id); // User id
            WriteInt(_user.Clan?.Id ?? 0); // Clan id
            WriteBool(false); // Temp: User is clan master
            WriteString(_user.Clan == null ? "" : _user.Clan.Name);
        }
    }
}