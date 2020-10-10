using Data.Model;

namespace GameServer.ServerPackets.Bridge.Stats
{   
    /// <summary>
    /// A packet containing stats for survival (deathmatch)
    /// </summary>
    public class SurvivalInfo : ServerBasePacket
    {
        private readonly UserStats _stats;
        
        public SurvivalInfo(UserStats stats)
        {
            _stats = stats;
        }
        
        public override string GetType()
        {
            return $"BRIDGE_SEND_SURVIVAL_INFO";
        }

        public override byte GetId()
        {
            return 0x99;
        }

        protected override void WriteImpl()
        {
            // Totally unknown
            // 12 Ints
            WriteInt(100);
            WriteInt(100);
            WriteInt(100); // Kills
            WriteInt(100); // Assist
            WriteInt(100); // Deaths
            WriteInt(100); // 1st place wins
            WriteInt(100); 
            WriteInt(100);
            WriteInt(100); // Desertions
            WriteInt(100); // Points
            WriteInt(100); // High Score
            WriteInt(100); // Time
            WriteInt(100);
        }
    }
}