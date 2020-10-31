namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent when a unit is repaired and ready to spawn
    /// </summary>
    public class UnitRepaired : ServerBasePacket
    {
        /// <summary>
        /// The unit that is ready
        /// </summary>
        private readonly int _unitId;

        public UnitRepaired(int unitId)
        {
            _unitId = unitId;
        }
        
        public override string GetType()
        {
            return "GAME_REGAIN_UNIT_REPAIRED";
        }

        public override byte GetId()
        {
            return 0x5a;
        }

        protected override void WriteImpl()
        {
            WriteInt(_unitId);
        }
    }
}