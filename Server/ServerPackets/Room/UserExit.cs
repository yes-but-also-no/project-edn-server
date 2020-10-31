using Data.Model;

namespace GameServer.ServerPackets.Room
{
    public class UserExit : ServerBasePacket
    {
        private readonly ExteelUser _user;

        public UserExit(ExteelUser user)
        {
            _user = user;
        }
        
        public override string GetType()
        {
            return "ROOM_USER_EXIT";
        }

        public override byte GetId()
        {
            return 0x3a;
        }

        protected override void WriteImpl()
        {
            WriteInt(_user.Id); // User id?
            WriteInt(0); // Result?
        }
    }
}