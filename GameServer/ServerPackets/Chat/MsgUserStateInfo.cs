using Data.Model;

namespace GameServer.ServerPackets.Chat
{
    /// <summary>
    /// Sends a users state info
    /// </summary>
    public class MsgUserStateInfo : ServerBasePacket
    {
        /// <summary>
        /// The user
        /// </summary>
        private readonly ExteelUser _user;
        
        public MsgUserStateInfo(ExteelUser user)
        {
            _user = user;
        }
        
        public override string GetType()
        {
            return "MSG_USER_STATE_INFO";
        }

        public override byte GetId()
        {
            return 0xd1;
        }

        protected override void WriteImpl()
        {
            WriteInt(_user.Id); // Id
            WriteBool(true); // Temp: User is online
            WriteString("ChatChannel"); // Temp: Chat channel user is in
        }
    }
}