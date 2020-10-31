using Data.Model;

namespace GameServer.ServerPackets.Bridge.Stats
{   
    /// <summary>
    /// A packet containing stats for survival (deathmatch)
    /// </summary>
    public class DefensiveBattleInfo : ServerBasePacket
    {
        private readonly UserStats _stats;
        
        public DefensiveBattleInfo(UserStats stats)
        {
            _stats = stats;
        }
        
        public override string GetType()
        {
            return $"BRIDGE_SEND_DEFENSIVEBATTLE_INFO";
        }

        public override byte GetId()
        {
            return 0x9e;
        }

        protected override void WriteImpl()
        {
            // Totally unknown
            // 8 Ints
            WriteInt(100); // 
            WriteInt(100); // 
            WriteInt(100); // Wins
            WriteInt(100); // Desertions
            WriteInt(100); // Losses
            WriteInt(100); // Points
            WriteInt(100); // NPC Kills
            WriteInt(100); // Time
            
            
            WriteInt(1); // Size ?
            
            WriteInt(0); // Map ID
            WriteInt(100); // Total record (points?)
            WriteInt(100); // Total ranking (leaderboard?)
            WriteInt(100); // Weekly record
            WriteInt(100); // Weekly ranking
            WriteInt(100); // Last weeks record
            WriteInt(100); // Last weeks ranking
            WriteInt(100); // Last weeks credits earned
        }
    }
}