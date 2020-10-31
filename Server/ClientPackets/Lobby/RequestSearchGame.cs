using System;
using System.Drawing;
using System.Linq;
using GameServer.Managers;
using GameServer.ServerPackets.Lobby;
using Console = Colorful.Console;

namespace GameServer.ClientPackets.Lobby
{
    /// <summary>
    /// Called when the user refreshes their lobby games list
    /// </summary>
    public class RequestSearchGame : ClientBasePacket
    {
        public RequestSearchGame(byte[] data, GameSession client) : base(data, client)
        {
//            Console.WriteLine("Int?? - : {0}", GetInt()); // ??
//            Console.WriteLine("Int?? - : {0}", GetInt()); // ??
        }

        public override string GetType()
        {
            return "LOBBY_REQ_SEARCH_GAME";
        }

        protected override void RunImpl()
        {
            var rooms = RoomManager.GetRooms();
            // Search for rooms
            // TODO: Check the paramaters in this packet to see if they are filters or something
            
            //Console.WriteLine($"Sending {rooms.Count} to user {GetClient().GetUserName()}");
            
            GetClient().SendPacket(new GameSearched(rooms));
        }
    }
}