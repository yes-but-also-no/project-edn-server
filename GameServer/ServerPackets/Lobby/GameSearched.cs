using System.Collections.Generic;
using GameServer.Game;

namespace GameServer.ServerPackets.Lobby
{
    /// <summary>
    /// Response to a game search request
    /// </summary>
    public class GameSearched : ServerBasePacket
    {
        /// <summary>
        /// The rooms that were found
        /// </summary>
        private readonly List<RoomInstance> _rooms;
        
        public GameSearched(List<RoomInstance> rooms)
        {
            _rooms = rooms;
        }
        
        public override string GetType()
        {
            return "LOBBY_GAME_SEARCHED";
        }

        public override byte GetId()
        {
            return 0x2f;
        }

        protected override void WriteImpl()
        {
            WriteInt(_rooms.Count);

            foreach (var room in _rooms)
            {
                this.WriteRoomInfo(room);
            }
        }
    }
}