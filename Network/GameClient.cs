﻿using System;
using System.Net.Sockets;
using Sylver.Network.Data;
using Sylver.Network.Server;

namespace Network
{
    /// <summary>
    /// This represents a single connect net client
    /// </summary>
    public class GameClient : NetServerClient
    {
        public GameClient(Socket socketConnection) : base(socketConnection)
        {
        }

        public override void HandleMessage(INetPacketStream packet)
        {
            var packetId = packet.Read<byte>();

            if (packetId == 0x0d)
            {
                var str = packet.Read<string>();
            }

            //throw new NotImplementedException();
        }
    }
}