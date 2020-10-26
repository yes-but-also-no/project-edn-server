using Console = Colorful.Console;

namespace GameServer.ClientPackets.Game
{
    /// <summary>
    /// Called when the client enters sniper mode
    /// </summary>
    public class ModeSniper : ClientGameBasePacket
    {
        public ModeSniper(byte[] data, GameSession client) : base(data, client)
        {
            TickUnit();
            
            // Read the units position and aim
            GetUnitPositionAndAim();
            
            Console.WriteLine("Sniper unknown byte {0}", GetByte());
        }

        public override string GetType()
        {
            return "MODE_SNIPER";
        }

        protected override void RunImpl()
        {
            Unit?.TrySetSniperMode(true);
        }
    }
}