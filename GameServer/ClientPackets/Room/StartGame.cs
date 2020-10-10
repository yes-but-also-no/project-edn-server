namespace GameServer.ClientPackets.Room
{
    /// <summary>
    /// Sent when the user wishes to start the game
    /// </summary>
    public class StartGame : ClientBasePacket
    {
        public StartGame(byte[] data, GameSession client) : base(data, client)
        {
        }

        public override string GetType()
        {
            return "ROOM_START_GAME";
        }

        protected override void RunImpl()
        {
            // TODO: Check errors - users not ready, team balance, etc
            var client = GetClient();
            
            client.RoomInstance.TryStartGame();
        }
    }
}