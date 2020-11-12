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
        
        #region ROOM
        
        RoomUserEnter = 0x0a,
        
        #endregion

        #region INVENTORY

        InventoryMoney = 0x1d,
        InventoryDefaultUnit = 0x1e,
        InventoryCodeList = 0x1f,
        InventoryParts = 0x20,
        InventoryUnitSlots = 0x21,
        InventoryUnitInfo = 0x22,
        InventoryPalette = 0x23,
        InventoryOperatorList = 0x24,
        InventorySpecialItems = 0x25,
        InventoryEnd = 0x26,

        #endregion
        
        #region LOBBY
        
        GameCreated = 0x32,
        GameEntered = 0x33,
        
        #endregion
        
        #region SHOP
        
        SendGoodsDataStart = 0x90,
        SendGoodsData = 0x91,
        SendGoodsDataEnd = 0x92,
        
        #endregion

        #region BRIDGE

        SendAvatarInfo = 0x97,
        SendTrainingInfo = 0x98,
        SendSurvivalInfo = 0x99,
        SendTeamSurvivalInfo = 0x9a,
        SendTeamBattleInfo = 0x9b,
        SendCtfInfo = 0x9c,
        SendClanBattleInfo = 0x9d,
        SendDefensiveBattleInfo = 0x9e,
        SendBestInfo = 0x9f

        #endregion
        
    }
}