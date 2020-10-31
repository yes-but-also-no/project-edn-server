using Data.Model;

namespace GameServer.ServerPackets
{
    /// <summary>
    /// Probably sent as a response to buying an operator or selecting one
    /// </summary>
    public class SelectOperatorInfo : ServerBasePacket
    {
        // TODO: Pull actual operator from inv and handle result codes
        private readonly int _tempOp;

        public SelectOperatorInfo(int id)
        {
            _tempOp = id;
        }

        public override string GetType()
        {
            return "SELECT_OPERATOR_INFO";
        }

        public override byte GetId()
        {
            return 0xe6;
        }

        protected override void WriteImpl()
        {
            WriteInt(0); // Result
            WriteInt(_tempOp); // Id
        }
    }
}