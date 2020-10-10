namespace GameServer.ClientPackets.Chat
{
    /// <summary>
    /// I think this is always sent by client? Maybe it is requesting the current users clan info?
    /// Or maybe they just multicast all this?
    /// </summary>
    public class MsgUserClanInfo : ClientBasePacket
    {
        public MsgUserClanInfo(byte[] data, GameSession client) : base(data, client)
        {
            GetInt(); // Clan id
            GetBool(); // Is master
            GetString(); // Clan name
        }

        public override string GetType()
        {
            return "MSG_USER_CLAN_INFO";
        }

        protected override void RunImpl()
        {
            GetClient().SendPacket(new ServerPackets.Chat.MsgUserClanInfo(GetClient().User));
        }
    }
}