using System;
using System.Net.Sockets;
using Data.Configuration;
using Network.Packets.Client;
using Swan.Logging;
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

        /// <summary>
        /// Called when a packet is recieved from the client
        /// </summary>
        /// <param name="packet"></param>
        public override void HandleMessage(INetPacketStream packet)
        {

            byte packetId = 0;
            ClientPacketType packetType;

            try
            {
                packetId = packet.Read<byte>();

                packetType = (ClientPacketType) packetId;

                PacketHandler.Invoke(this, packet, packetType);

                if (ServerConfig.Configuration.Global.LogAllPackets)
                    $"[C] 0x{packetId:x2} {Enum.GetName(typeof(ClientPacketType), packetId)}".Info(ToString());
            }
            catch (PacketHandler.HandlerNotFoundException)
            {
                if (Enum.IsDefined(typeof(ClientPacketType), packetId))
                {
                    $"Received an unimplemented packet {Enum.GetName(typeof(ClientPacketType), packetId)} ({packetId:x2}) from {Socket.RemoteEndPoint}."
                        .Warn();
                }
                else
                {
                    $"Received an unknown packet {packetId:x2} from {Socket.RemoteEndPoint}.".Warn();
                }
            }
            catch (Exception exception)
            {
                $"Error occured in handle message from {Socket.RemoteEndPoint}".Error();
            }
        }

        // TODO: Username
        public override string ToString()
        {
            return $"[GameClient]<{Id}><USERNAME_TODO>";
        }
    }
}