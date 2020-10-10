using System;
using GameServer.ServerPackets;

namespace GameServer.ClientPackets
{
    /// <summary>
    /// Sent when the client first connects to do an encryption handshake?
    /// </summary>
    public class ValidateClient : ClientBasePacket
    {
        public ValidateClient(byte[] data, GameSession client) : base(data, client)
        {
            Console.WriteLine("Validation: {0}", GetString());
        }

        public override string GetType()
        {
            return "VALIDATE_CLIENT";
        }

        protected override void RunImpl()
        {
            GetClient().SendPacket(new ClientValidated());
        }
    }
}