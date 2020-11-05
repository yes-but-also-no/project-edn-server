using System;
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
            // Get the session-id size
            var size = packet.Read<short>();

            // Read the bytes
            var bytes = packet.Read<byte>(size);
            
            // Convert to GUID
            SessionKey = new Guid(bytes);
        }

    }
}