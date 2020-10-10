namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent to start the tutorial?
    /// </summary>
    public class TutorialBegin : ServerBasePacket
    {
        public override string GetType()
        {
            return "TUTORIAL_BEGIN";
        }

        public override byte GetId()
        {
            return 0xb3;
        }

        protected override void WriteImpl()
        {
            // Do nothing?
        }
    }
}