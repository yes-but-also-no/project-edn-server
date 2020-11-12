using System;
using System.Reflection;
using Network;
using Network.Packets.Client;
using Network.Packets.Client.Core;

namespace Game.Handlers
{
    /// <summary>
    /// This class handles all room messages (ready / unready / etc)
    /// </summary>
    public static class RoomHandler
    {
        [PacketHandler(ClientPacketType.ListUser)]
        public static void OnListUser(GameClient client, LogPacket packet)
        {
            Console.WriteLine("TEST");
        }
    }
}