using System.Drawing;
using Colorful;

namespace GameServer.ClientPackets
{
    /// <summary>
    /// Logging packet from client
    /// </summary>
    public class Log : ClientBasePacket
    {
        /// <summary>
        /// Log data client sent us
        /// </summary>
        private readonly string _logString;
        
        public Log(byte[] data, GameSession client) : base(data, client)
        {
            _logString = GetString();
        }

        public override string GetType()
        {
            return "LOG";
        }

        protected override void RunImpl()
        {
            // TODO: If config C_LOG
            Console.WriteLine(_logString, Color.DodgerBlue);
        }
    }
}