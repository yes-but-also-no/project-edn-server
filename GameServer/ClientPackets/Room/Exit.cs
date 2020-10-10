namespace GameServer.ClientPackets.Room
{
    public class Exit : ClientBasePacket
    {
        /// <summary>
        /// Sent when the user wants to exit a room
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        public Exit(byte[] data, GameSession client) : base(data, client)
        {
        }

        public override string GetType()
        {
            return "ROOM_EXIT";
        }

        protected override void RunImpl()
        {
            GetClient().RoomInstance.TryLeaveGame(GetClient());
        }
    }
}