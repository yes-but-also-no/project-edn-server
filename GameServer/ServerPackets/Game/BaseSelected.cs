namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent as a reponse to users when they select a base
    /// Not fully understood
    /// </summary>
    public class BaseSelected : ServerBasePacket
    {
        public override string GetType()
        {
            return "GAME_BASE_SELECTED";
        }

        public override byte GetId()
        {
            return 0x55;
        }

        protected override void WriteImpl()
        {
            WriteInt(0); // Unknown
            WriteInt(0); // Unknown
        }
    }
}