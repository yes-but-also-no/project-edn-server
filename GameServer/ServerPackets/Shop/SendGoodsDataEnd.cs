namespace GameServer.ServerPackets.Shop
{
    public class SendGoodsDataEnd : ServerBasePacket
    {
        public override string GetType()
        {
            return "SEND_GOODS_DATA_END";
        }

        public override byte GetId()
        {
            return 0x92;
        }

        // Empty packet
        protected override void WriteImpl() {}
    }
}