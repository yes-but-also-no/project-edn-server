using Sylver.Network.Data;

namespace Network.Packets
{
    /// <summary>
    /// Provides an interface to serialize an object.
    /// </summary>
    public interface IPacketSerializer
    {
        /// <summary>
        /// Serializes the current object.
        /// </summary>
        /// <param name="packet">Packet stream.</param>
        void Serialize(INetPacketStream packet);
    }
}