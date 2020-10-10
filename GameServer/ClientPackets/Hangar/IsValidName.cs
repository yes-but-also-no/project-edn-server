using GameServer.ServerPackets.Hangar;

namespace GameServer.ClientPackets.Hangar
{
    /// <summary>
    /// Sent when user saves their mech to validate name
    /// </summary>
    public class IsValidName : ClientBasePacket
    {
        /// <summary>
        /// The name they entered
        /// </summary>
        private readonly string _name;
        
        public IsValidName(byte[] data, GameSession client) : base(data, client)
        {
            _name = GetString();
        }

        public override string GetType()
        {
            return "IS_VALID_NAME";
        }

        protected override void RunImpl()
        {
            // TODO: Check name
            GetClient().SendPacket(new NameIsValid(true));
        }
    }
}