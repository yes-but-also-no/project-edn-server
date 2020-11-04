using Network;
using Network.Packets;
using Swan.Logging;

namespace GameServer.New
{
    /// <summary>
    /// Test packet handler
    /// </summary>
    public static class TestManager
    {
        [PacketHandler(0x0d)]
        public static void OnLog(GameClient client, LogPacket packet)
        {
            packet.LogMessage.Info();
        }
    }
}