using GameServer.Game;

namespace GameServer.ServerPackets.Room
{
    /// <summary>
    /// Sent when the user trys to start the game
    /// </summary>
    public class GameStart : ServerBasePacket
    {
        /// <summary>
        /// The room
        /// </summary>
        private readonly RoomInstance _room;

        /// <summary>
        /// The result code of this action
        /// </summary>
        private readonly int _resultCode;
        
        public GameStart(RoomInstance room, int resultCode)
        {
            _room = room;
            _resultCode = resultCode;
        }
        
        public override string GetType()
        {
            return "ROOM_GAME_START";
        }

        public override byte GetId()
        {
            return 0x3d;
        }

        protected override void WriteImpl()
        {
            // Result code
            WriteInt(_resultCode);
            
            this.WriteRoomInfo(_room);
        }
    }
}