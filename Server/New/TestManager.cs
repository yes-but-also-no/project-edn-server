using Network;
using Network.Packets.Client;
using Network.Packets.Client.Core;
using Swan.Logging;

namespace GameServer.New
{
    /// <summary>
    /// Test packet handler
    /// </summary>
    public static class TestManager
    {
        [PacketHandler(ClientPacketType.Log)]
        public static void OnLog(GameClient client, LogPacket packet)
        {
            packet.LogMessage.Info();
        }
    }
}