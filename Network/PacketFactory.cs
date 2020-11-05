using Network.Packets;

namespace Network
{
    /// <summary>
    /// This class handles all outgoing packets
    /// Including headers and serialization
    /// </summary>
    public static class PacketFactory
    {
        /// <summary>
        /// Creates a server packet
        /// </summary>
        /// <param name="packetData"></param>
        /// <returns></returns>
        public static GamePacket CreatePacket(IPacketSerializer packetData)
        {
            // Create packet
            var packet = new GamePacket(packetData.PacketType);
            
            // Serialize
            packetData.Serialize(packet);

            // Return
            return packet;
        }
    }
}