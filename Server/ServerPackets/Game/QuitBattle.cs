namespace GameServer.ServerPackets.Game
{
    public class QuitBattle : ServerBasePacket
    {
        /// <summary>
        /// Sent to the client as a response to them quitting the battle
        /// </summary>
        /// <returns></returns>
        public override string GetType()
        {
            return "QUIT_BATTLE";
        }

        public override byte GetId()
        {
            return 0x58;
        }

        protected override void WriteImpl()
        {
            WriteInt(-1); // Pulled from packet capture. unsure of purpose.
        }
    }
}