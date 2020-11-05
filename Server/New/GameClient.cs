using System;
using System.Net.Sockets;
using Data.Configuration;
using Network;
using Network.Packets.Client;
using Swan.Logging;
using Sylver.Network.Data;
using Sylver.Network.Server;

namespace GameServer.New
{
    /// <summary>
    /// This represents a single connect net client
    /// </summary>
    public class GameClient : NetServerClient
    {
        public GameClient(Socket socketConnection) : base(socketConnection)
        {
        }

        /// <summary>
        /// Called when a packet is recieved from the client
        /// </summary>
        /// <param name="packet"></param>
        public override void HandleMessage(INetPacketStream packet)
        {

            // Temporary variables
            byte packetId = 0;
            ClientPacketType packetType;

            try
            {
                // Read packed id
                packetId = packet.Read<byte>();

                // Attempt to cast to packet id
                packetType = (ClientPacketType) packetId;

                // Invoke handler
                PacketHandler<GameClient>.Invoke(this, packet, packetType);

                // Log
                if (ServerConfig.Configuration.Global.LogAllPackets)
                    $"[C] 0x{packetId:x2} {Enum.GetName(typeof(ClientPacketType), packetId)}".Info(ToString());
            }
            catch (PacketHandler<GameClient>.HandlerNotFoundException)
            {
                // Check for missing handler
                if (Enum.IsDefined(typeof(ClientPacketType), packetId))
                {
                    $"Received an unimplemented packet {Enum.GetName(typeof(ClientPacketType), packetId)} ({packetId:x2}) from {Socket.RemoteEndPoint}."
                        .Warn(ToString());
                }
                else
                {
                    $"Received an unknown packet {packetId:x2} from {Socket.RemoteEndPoint}.".Warn(ToString());
                }
            }
            catch (Exception exception)
            {
                // All other errors
                $"Error occured in handle message from {Socket.RemoteEndPoint}".Error(ToString());
            }
        }

        // TODO: Username
        public override string ToString()
        {
            return $"[GameClient]<{Id}><USERNAME_TODO>";
        }
    }
}