using Data.Model;

namespace GameServer.ServerPackets.Bridge
{   
    /// <summary>
    /// A packet containing stats for a particular mode of play
    /// </summary>
    public class BestInfo : ServerBasePacket
    {
        /// <summary>
        /// The user this stat info is for
        /// </summary>
        private readonly ExteelUser _user;
        
        public BestInfo(ExteelUser user)
        {
            _user = user;
        }
        
        public override string GetType()
        {
            return $"BRIDGE_SEND_BEST_INFO";
        }

        public override byte GetId()
        {
            return 0x9f;
        }

        protected override void WriteImpl()
        {
            // Overall has a mysterious string and int header
            WriteInt(0);
            WriteString("OverallInfo");
            
            // Totally unknown
            // 12 Ints
            WriteInt(100);
            WriteInt(100);
            WriteInt(100); 
            WriteInt(100); 
            WriteInt(100); // Kills
            WriteInt(100); // Deaths
            WriteInt(100); // Assist
            WriteInt(100);
            WriteInt(100); // Wins
            WriteInt(100); // Lose
            WriteInt(100); // Points
            WriteInt(100);// Best
            WriteInt(100);
        }
    }
}