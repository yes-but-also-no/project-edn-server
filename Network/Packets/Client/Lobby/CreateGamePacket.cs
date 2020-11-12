using Data.Model;
using Sylver.Network.Data;

namespace Network.Packets.Client.Lobby
{
    public class CreateGamePacket : IPacketDeserializer
    {

        /// <summary>
        /// A partial room record for the room they are trying to create
        /// </summary>
        public RoomRecord RoomRecord { get; private set; }

        public void Deserialize(GamePacket packet)
        {
            RoomRecord = new RoomRecord
            {
                Name = packet.ReadGameString(),
                Password = packet.ReadGameString().Trim('\0'),
                TemplateId = packet.Read<uint>(),
                Difficulty = packet.Read<int>(),
            
                Balance = packet.Read<bool>(),
                FiveMinute = packet.Read<bool>(),
                SdMode = packet.Read<bool>(),
                Visible = packet.Read<bool>(),
            
                MaxLevel = packet.Read<int>(),
                MinLevel = packet.Read<int>()
            };
            
            
        }

    }
}