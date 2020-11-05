using GameServer.New;
using Network;
using Network.Packets.Client;
using Network.Packets.Client.Core;
using Network.Packets.Server.Core;
using Swan.Logging;
using ClientTimePacket = Network.Packets.Client.Core.ServerTimePacket;
using ServerTimePacket = Network.Packets.Server.Core.ServerTimePacket;

namespace GameServer.Handlers
{
    /// <summary>
    /// This class will manage all core packets and game functionality
    /// </summary>
    public static class CoreHandler
    {
        /// <summary>
        /// Called when the client wants to check their protocol version against the servers
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(ClientPacketType.ProtocolVersion)]
        public static void OnClientVersion(GameClient client, ProtocolVersionPacket packet)
        {
            // Log
            $"Client protocol check request. Client Protocol: [{packet.Version}]".Debug(client.ToString());
            
            // TODO: Validate client protocol
            
            // Return success
            client.Send(PacketFactory.CreatePacket(new ServerVersionPacket()));
        }
        
        /// <summary>
        /// Called when the client wishes to validate their client data
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(ClientPacketType.ValidateClient)]
        public static void OnValidateClient(GameClient client, ValidateClientPacket packet)
        {
            // Log
            $"Client validation request. Seed: [{packet.ValidationSeed}]".Debug(client.ToString());
            
            // TODO: Validate client
            
            // Return success
            client.Send(PacketFactory.CreatePacket(new ClientValidatedPacket()));
        }
        
        /// <summary>
        /// Called when the client wishes verify the server time
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(ClientPacketType.ServerTime)]
        public static void OnServerTime(GameClient client, ClientTimePacket packet)
        {
            // Return server time
            client.Send(PacketFactory.CreatePacket(new ServerTimePacket
            {
                ClientTime = packet.ClientTime,
                ServerTime = NewServer.RunningMs
            }));
        }
        
        /// <summary>
        /// Called when the client would like to log data
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(ClientPacketType.Log)]
        public static void OnLog(GameClient client, LogPacket packet)
        {
            packet.LogMessage.Debug(client.ToString());
        }
    }
}