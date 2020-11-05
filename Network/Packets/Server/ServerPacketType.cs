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
        SendGoodsDataEnd = 0x92
    }
}