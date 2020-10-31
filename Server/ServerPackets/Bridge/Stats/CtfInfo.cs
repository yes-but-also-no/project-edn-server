using Data.Model;

namespace GameServer.ServerPackets.Bridge.Stats
{   
    /// <summary>
    /// A packet containing stats for team battle (territory control)
    /// </summary>
    public class CtfInfo : ServerBasePacket
    {
        private readonly UserStats _stats;
        
        public CtfInfo(UserStats stats)
        {
            _stats = stats;
        }
        
        public override string GetType()
        {
            return $"BRIDGE_SEND_CTF_INFO";
        }

        public override byte GetId()
        {
            return 0x9c;
        }

        protected override void WriteImpl()
        {
            // Totally unknown
            // 12 Ints
            WriteInt(100); 
            WriteInt(100);
            WriteInt(100); // Kills 
            WriteInt(100); // Assists
            WriteInt(100); // Deaths
            WriteInt(100); // Wins
            WriteInt(100); // Losses
            WriteInt(100); // Draws
            WriteInt(100); // Desertions
            WriteInt(100); // Flag captures
            WriteInt(100); // 
            WriteInt(100); // 
            WriteInt(100); // 
            WriteInt(100); // 
            WriteInt(100); // 
        }
    }
}