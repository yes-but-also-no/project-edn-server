using Data.Model;

namespace GameServer.ServerPackets.Bridge.Stats
{   
    /// <summary>
    /// A packet containing stats for team survival (team deathmatch)
    /// </summary>
    public class TeamSurvivalInfo : ServerBasePacket
    {
        private readonly UserStats _stats;
        
        public TeamSurvivalInfo(UserStats stats)
        {
            _stats = stats;
        }
        
        public override string GetType()
        {
            return $"BRIDGE_SEND_TEAMSURVIVAL_INFO";
        }

        public override byte GetId()
        {
            return 0x9a;
        }

        protected override void WriteImpl()
        {
            // Totally unknown
            // 12 Ints
            WriteInt(100); 
            WriteInt(100);// 
            WriteInt(100); // Kills
            WriteInt(100); // Assists
            WriteInt(100); // 
            WriteInt(100); // Deaths
            WriteInt(100); // Wins
            WriteInt(100); // Losses
            WriteInt(100); // Draws
            WriteInt(100); // Desertions
            WriteInt(100); // Points
            WriteInt(100); // High Score
            WriteInt(100); // Time
        }
    }
}