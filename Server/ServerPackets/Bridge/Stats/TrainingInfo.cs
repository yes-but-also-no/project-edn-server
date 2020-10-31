using Data.Model;

namespace GameServer.ServerPackets.Bridge.Stats
{   
    /// <summary>
    /// A packet containing stats for survival (deathmatch)
    /// </summary>
    public class TrainingInfo : ServerBasePacket
    {
        private readonly UserStats _stats;
        
        public TrainingInfo(UserStats stats)
        {
            _stats = stats;
        }
        
        public override string GetType()
        {
            return $"BRIDGE_SEND_TRAINING_INFO";
        }

        public override byte GetId()
        {
            return 0x98;
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