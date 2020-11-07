using Data.Model;
using GameServer.Game;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent to all clients in a room to give a users info
    /// </summary>
    public class UserInfo : ServerBasePacket
    {
        /// <summary>
        /// The user this is for
        /// </summary>
        private readonly ExteelUser _user;

        private readonly GameInstance _room;

        public UserInfo(GameInstance room, ExteelUser user)
        {
            HighPriority = true;
            
            _user = user;
            _room = room;
        }
        
        public override string GetType()
        {
            return "GAME_USER_INFO";
        }

        public override byte GetId()
        {
            return 0x49;
        }

        protected override void WriteImpl()
        {
            this.WriteRoomUserInfo(_room?.RoomInstance, _user);
        }
    }
}