using System;

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
        public byte Header { get; private set; }

        /// <summary>
        /// Creates a new <see cref="PacketHandlerAttribute"/> instance.
        /// </summary>
        /// <param name="header"></param>
        public PacketHandlerAttribute(byte header)
        {
            Header = header;
        }
    }
}