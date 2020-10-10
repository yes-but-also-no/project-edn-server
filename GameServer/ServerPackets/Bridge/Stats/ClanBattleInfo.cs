using Data.Model;

namespace GameServer.ServerPackets.Bridge.Stats
{   
    /// <summary>
    /// A packet containing stats for survival (deathmatch)
    /// </summary>
    public class ClanBattleInfo : ServerBasePacket
    {
        private readonly UserStats _stats;
        
        public ClanBattleInfo(UserStats stats)
        {
            _stats = stats;
        }
        
        public override string GetType()
        {
            return $"BRIDGE_SEND_CLANBATTLE_INFO";
        }

        public override byte GetId()
        {
            return 0x9d;
        }

        protected override void WriteImpl()
        {
            // Totally unknown
            // 12 Ints
            WriteInt(100);
            WriteInt(100);
            WriteInt(100); 
            WriteInt(100); 
            WriteInt(100); 
            WriteInt(100); 
            WriteInt(100); 
            WriteInt(100);
            WriteInt(100); 
            WriteInt(100); 
            WriteInt(100); 
            WriteInt(100);
            WriteInt(100);
        }
    }
}