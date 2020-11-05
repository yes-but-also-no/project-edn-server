using Sylver.Network.Data;

namespace Network.Packets.Client.Core
{
    public class ValidateClientPacket : IPacketDeserializer
    {

        /// <summary>
        /// Unknown validation seed, somehow related to client files?
        /// </summary>
        public string ValidationSeed { get; private set; }

        public void Deserialize(GamePacket packet)
        {
            ValidationSeed = packet.ReadGameString();
        }

    }
}