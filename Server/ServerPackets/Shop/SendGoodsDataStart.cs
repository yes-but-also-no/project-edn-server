namespace GameServer.ServerPackets.Shop
{
    /// <summary>
    /// Sends the start indicator for the goods data download, along with the number of goods
    /// </summary>
    public class SendGoodsDataStart : ServerBasePacket
    {
        // TODO: This should be the actual number of goods
        private readonly int _goodsSize = 9;
        
        public override string GetType()
        {
            return "SEND_GOODS_DATA_START";
        }

        public override byte GetId()
        {
            return 0x90;
        }

        protected override void WriteImpl()
        {
            WriteInt(_goodsSize);
        }
    }
}