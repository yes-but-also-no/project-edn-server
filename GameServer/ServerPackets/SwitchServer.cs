namespace GameServer.ServerPackets
{
    /// <summary>
    /// Sent to the user to allow them to switch servers
    /// </summary>
    public class SwitchServer : ServerBasePacket
    {
        /// <summary>
        /// The job code for the user to switch over on
        /// </summary>
        private readonly int _jobCode;
        
        public SwitchServer(int jobCode)
        {
            _jobCode = jobCode;
        }
        
        public override string GetType()
        {
            return "SWITCH_SERVER";
        }

        public override byte GetId()
        {
            return 0x0e;
        }

        protected override void WriteImpl()
        {
            WriteInt(0); // Unknown - return code?
            
            WriteString("isishqduarte.mynetgear.com"); // Host
            
            WriteInt(_jobCode); // Unknown
        }
    }
}