namespace Network.Packets.Server
{
    /// <summary>
    /// This has all packet types
    /// </summary>
    public enum ServerPacketType : byte
    {
        ServerVersion = 0x00,
        ClientValidated = 0x01,
        ConnectResult = 0x02,
        ServerTime = 0x04,
        SendGoodsDataStart = 0x90,
        SendGoodsData = 0x91,
        SendGoodsDataEnd = 0x92,
        SendAvatarInfo = 0x97,
        SendTrainingInfo = 0x98,
        SendSurvivalInfo = 0x99,
        SendTeamSurvivalInfo = 0x9a,
        SendTeamBattleInfo = 0x9b,
        SendCtfInfo = 0x9c,
        SendClanBattleInfo = 0x9d,
        SendDefensiveBattleInfo = 0x9e,
        SendBestInfo = 0x9f
    }
}