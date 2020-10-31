using Data.Model;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent when a user leaves the game
    /// </summary>
    public class UserLeave : ServerBasePacket
    {
        /// <summary>
        /// The user who is leaving
        /// </summary>
        private ExteelUser _user;
        
        public UserLeave(ExteelUser user)
        {
            _user = user;
        }
        
        public override string GetType()
        {
            return "USER_LEAVE";
        }

        public override byte GetId()
        {
            return 0x0b;
        }

        protected override void WriteImpl()
        {
            WriteInt(_user.Id);
        }
    }
}