using GameServer.New;
using Network;
using Network.Packets.Client;
using Network.Packets.Client.Bridge;
using Network.Packets.Server.Bridge;

namespace GameServer.Handlers
{
    /// <summary>
    /// This class handles all packets related to bridge, stats, etc
    /// </summary>
    public static class BridgeHandler
    {
        /// <summary>
        /// Called when the client would like to get their avatar info
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(ClientPacketType.RequestAvatarInfo)]
        public static void OnRequestAvatarInfo(GameClient client, RequestAvatarInfoPacket packet)
        {
            // Send response packet
            client.Send(PacketFactory.CreatePacket(new SendAvatarInfoPacket
            {
                User = client.User
            }));
        }
    }
}