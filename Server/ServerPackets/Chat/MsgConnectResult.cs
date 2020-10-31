namespace GameServer.ServerPackets.Chat
{
    /// <summary>
    /// Sent to confirm message connection success
    /// </summary>
    public class MsgConnectResult : ServerBasePacket
    {
        public override string GetType()
        {
            return "MSG_CONNECT_RESULT";
        }

        public override byte GetId()
        {
            return 0xd0;
        }

        protected override void WriteImpl()
        {
            WriteInt(0); // 0 for success
        }
    }
}