using Data.Model;
using GameServer.Game;

namespace GameServer.ServerPackets.Room
{
    /// <summary>
    /// Contains the public user info for users in a room
    /// </summary>
    public class UserInfo : ServerBasePacket
    {
        /// <summary>
        /// The user contained in this packet
        /// </summary>
        private readonly ExteelUser _user;

        private readonly RoomInstance _room;

        public UserInfo(RoomInstance room, ExteelUser user)
        {
            _room = room;
            _user = user;
        }
        
        public override string GetType()
        {
            return "ROOM_USER_INFO";
        }

        public override byte GetId()
        {
            return 0x35;
        }

        protected override void WriteImpl()
        {
            this.WriteRoomUserInfo(_room, _user);
        }
    }
}