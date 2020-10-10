using System;
using System.Drawing;
using System.Linq;
using Data.Model;
using GameServer.Managers;
using GameServer.ServerPackets.Lobby;
using Console = Colorful.Console;

namespace GameServer.ClientPackets.Lobby
{
    /// <summary>
    /// Called when a user enters a game after server switch
    /// </summary>
    public class EnterGame : ClientBasePacket
    {
        /// <summary>
        /// The room Id in number form
        /// </summary>
        private readonly int _roomId;
        
        public EnterGame(byte[] data, GameSession client) : base(data, client)
        {
            _roomId = GetInt();
//            var unknown = GetInt();
        }

        public override string GetType()
        {
            return "LOBBY_ENTER_GAME";
        }

        protected override void RunImpl()
        {
            var room = RoomManager.GetRoomById(_roomId);
            var client = GetClient();
            
            GetClient().SendPacket(room.TryEnterGame(client));
        }
    }
}