namespace GameServer.ServerPackets.Hangar
{
    /// <summary>
    /// Sent to acknowledge the unit edit is complete?
    /// </summary>
    public class FactoryModifyEnd : ServerBasePacket
    {
        public override string GetType()
        {
            return "FACTORY_MODIFY_END";
        }

        public override byte GetId()
        {
            return 0x2c;
        }

        protected override void WriteImpl()
        {
            // Do nothing
        }
    }
}