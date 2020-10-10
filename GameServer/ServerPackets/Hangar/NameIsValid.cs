namespace GameServer.ServerPackets.Hangar
{
    /// <summary>
    /// Sent to validate mech name
    /// </summary>
    public class NameIsValid : ServerBasePacket
    {
        private readonly bool _isValid;
        
        public NameIsValid(bool isValid)
        {
            _isValid = isValid;
        }
        
        public override string GetType()
        {
            return "NAME_IS_VALID";
        }

        public override byte GetId()
        {
            return 0x2d;
        }

        protected override void WriteImpl()
        {
            WriteInt(_isValid ? 0 : 1);
        }
    }
}