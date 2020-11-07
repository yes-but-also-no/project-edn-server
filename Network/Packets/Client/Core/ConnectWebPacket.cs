using System;
using System.Linq;
using Swan.Logging;
using Sylver.Network.Data;

namespace Network.Packets.Client.Core
{
    /// <summary>
    /// There is other paramters to this packet, like mac address, gameid? etc that can be reversed
    /// </summary>
    public class ConnectWebPacket : IPacketDeserializer
    {

        /// <summary>
        /// The session key this client is trying to use to log in
        /// </summary>
        public Guid SessionKey { get; private set; }        

        public void Deserialize(GamePacket packet)
        {
            // Throw away
            packet.Read<short>();

            // Read guid
            var a = packet.Read<byte>(4).Reverse();
            var b = packet.Read<byte>(2).Reverse();
            var c = packet.Read<byte>(2).Reverse();
            var bytes = packet.Read<byte>(8);
            
            // Convert to GUID
            SessionKey = new Guid(a.Concat(b).Concat(c).Concat(bytes).ToArray());
        }

    }
}