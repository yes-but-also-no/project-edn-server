using GameServer.Game;

namespace GameServer.ServerPackets.Lobby
{
    /// <summary>
    /// Sent when a game is successfully created
    /// </summary>
    public class GameCreated : ServerBasePacket
    {
        /// <summary>
        /// Room that was created
        /// </summary>
        private readonly RoomInstance _room;

        public GameCreated(RoomInstance room)
        {
            _room = room;
        }
        
        public override string GetType()
        {
            return "LOBBY_GAME_CREATED";
        }

        public override byte GetId()
        {
            return 0x32;
        }

        protected override void WriteImpl()
        {
            WriteInt(0); // Result code
            WriteInt(_room.Id); // Session id?

            this.WriteRoomInfo(_room);
            
            WriteString("isishqduarte.mynetgear.com"); // host?
        }
    }
}