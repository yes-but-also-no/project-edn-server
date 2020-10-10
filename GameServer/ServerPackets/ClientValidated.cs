namespace GameServer.ServerPackets
{
    /// <summary>
    /// Sent to the client when they are validated?
    /// </summary>
    public class ClientValidated : ServerBasePacket
    {
        public override string GetType()
        {
            return "CLIENT_VALIDATED";
        }

        public override byte GetId()
        {
            return 0x01;
        }

        protected override void WriteImpl()
        {
            //WriteInt(1);
        }
    }
}