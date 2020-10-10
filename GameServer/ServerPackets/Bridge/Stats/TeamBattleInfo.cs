using Data.Model;

namespace GameServer.ServerPackets.Bridge.Stats
{   
    /// <summary>
    /// A packet containing stats for team battle (territory control)
    /// </summary>
    public class TeamBattleInfo : ServerBasePacket
    {
        private readonly UserStats _stats;
        
        public TeamBattleInfo(UserStats stats)
        {
            _stats = stats;
        }
        
        public override string GetType()
        {
            return $"BRIDGE_SEND_TEAMBATTLE_INFO";
        }

        public override byte GetId()
        {
            return 0x9b;
        }

        protected override void WriteImpl()
        {
            // Totally unknown
            // 12 Ints
            WriteInt(100); 
            WriteInt(100);
            WriteInt(100); // Kills 
            WriteInt(100); // Assists
            WriteInt(100); // Aerogate (captures?)
            WriteInt(100); 
            WriteInt(100); // Deaths
            WriteInt(100); // Wins
            WriteInt(100); // Losses
            WriteInt(100); // Draws
            WriteInt(100); // Desertions
            WriteInt(100); // Points
            WriteInt(100); // High score
            WriteInt(100); // Time
            WriteInt(100); // 
        }
    }
}