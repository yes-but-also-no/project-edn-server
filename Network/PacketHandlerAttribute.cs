using System;
using Network.Packets.Client;

namespace Network
{
    /// <summary>
    /// This attribute can be applied to methods in a manager / handler to allow it to
    /// act on packets
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class PacketHandlerAttribute : Attribute
    {
        /// <summary>
        /// Gets the packet attribute header.
        /// </summary>
        public ClientPacketType Header { get; private set; }

        /// <summary>
        /// Creates a new <see cref="PacketHandlerAttribute"/> instance.
        /// </summary>
        /// <param name="header"></param>
        public PacketHandlerAttribute(ClientPacketType header)
        {
            Header = header;
        }
    }
}