namespace GameServer.ClientPackets.Hangar
{
    /// <summary>
    /// Sent when the user is done modifying their unit
    /// </summary>
    public class FactoryModifyEnd : ClientBasePacket
    {
        public FactoryModifyEnd(byte[] data, GameSession client) : base(data, client)
        {
            debug = true;
        }

        public override string GetType()
        {
            return "FACTORY_MODIFY_END";
        }

        protected override void RunImpl()
        {
            GetClient().SendPacket(new ServerPackets.Hangar.FactoryModifyEnd());
        }
    }
}