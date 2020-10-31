namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent when all units are loaded in and the game is ready to begin
    /// </summary>
    public class GameStarted : ServerBasePacket
    {
        public override string GetType()
        {
            return "GAME_STARTED";
        }

        public override byte GetId()
        {
            return 0x4e;
        }

        protected override void WriteImpl()
        {
            WriteInt(0); // Unknown
            WriteInt(0); // Unknown
        }
    }
}