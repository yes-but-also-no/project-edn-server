namespace GameServer.ClientPackets.Game
{
    /// <summary>
    /// Sent frequently by the user to indicate their current position
    /// </summary>
    public class MoveUnit : ClientGameBasePacket
    {
        private readonly (byte, byte, byte) _movement;
        
        public MoveUnit(byte[] data, GameSession client) : base(data, client)
        {
            HighPriority = true;
            
            TickUnit();

            _movement = (GetByte(), GetByte(), GetByte());

            // Read the position information
            GetUnitPositionAndAim();
        }

        public override string GetType()
        {
            return "GAME_MOVE_UNIT";
        }

        protected override void RunImpl()
        {
            Unit.Move(_movement);
        }
    }
}