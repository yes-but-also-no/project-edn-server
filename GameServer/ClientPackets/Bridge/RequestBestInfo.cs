using GameServer.ServerPackets.Bridge;

namespace GameServer.ClientPackets.Bridge
{
    /// <summary>
    /// Looks to be a packet request user stats (kills, deaths, wins, etc)
    /// </summary>
    public class RequestBestInfo : ClientBasePacket
    {
        public RequestBestInfo(byte[] data, GameSession client) : base(data, client)
        {
        }

        public override string GetType()
        {
            return "BRIDGE_REQ_BEST_INFO";
        }

        protected override void RunImpl()
        {
            // Send various stats
            // TODO: Figure out which of these is which and how data is mapped

            var client = GetClient();
            
            client.SendPacket(new BestInfo(GetClient().User));
        }
    }
}