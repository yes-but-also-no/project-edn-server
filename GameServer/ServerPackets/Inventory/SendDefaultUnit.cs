namespace GameServer.ServerPackets.Inventory
{
    /// <summary>
    /// Sends the ID of the users default unit
    /// Maybe for lobby screen?
    /// </summary>
    public class SendDefaultUnit : ServerBasePacket
    {
        /// <summary>
        /// The id of the default unit
        /// </summary>
        private readonly int _defaultId;
        
        public SendDefaultUnit(int defaultId)
        {
            HighPriority = true;
            
            _defaultId = defaultId;
        }
        
        public override string GetType()
        {
            return "INV_SEND_DEFAULT_UNIT";
        }

        public override byte GetId()
        {
            return 0x1e;
        }

        protected override void WriteImpl()
        {
            WriteInt(_defaultId);
        }
    }
}