namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sends respawn timers to a client
    /// </summary>
    public class RegainInfo : ServerBasePacket
    {
        /// <summary>
        /// The session this info is for
        /// </summary>
        private readonly GameSession _session;

        /// <summary>
        /// Can this unit respawn yet?
        /// TODO: This should really be a repair mechanism for multiple units
        /// </summary>
        private readonly bool _canSpawn;
        
        public RegainInfo(GameSession session, bool canSpawn)
        {
            _session = session;
            _canSpawn = canSpawn;
        }
        
        public override string GetType()
        {
            return "GAME_REGAIN_INFO";
        }

        public override byte GetId()
        {
            return 0x59;
        }

        protected override void WriteImpl()
        {
            WriteInt(1); // Array size
            
            // TODO: Loop all units
            WriteInt(_session.User.DefaultUnit.Id); // Unit Id
            WriteInt(_session.User.DefaultUnit.LaunchOrder); // Launch (order)?
            WriteInt(0); // Life? - From client binaries
            WriteFloat(50.0f); // Repair? - From client
            WriteInt(4000); // Repair? Ms? - From client
            
        }
    }
}