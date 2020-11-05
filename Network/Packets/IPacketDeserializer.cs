using Sylver.Network.Data;

namespace Network.Packets
{
    /// <summary>
    /// Provides an interface to deserialize an object.
    /// </summary>
    public interface IPacketDeserializer
    {
        /// <summary>
        /// Deserializes the current packet stream.
        /// </summary>
        /// <param name="packet">Packet stream.</param>
        void Deserialize(GamePacket packet);
    }
}