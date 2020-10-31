using GameServer.Managers;

namespace GameServer.ServerPackets.Chat
{
    /// <summary>
    /// Sent to give the client a list of chat channels
    /// </summary>
    public class MsgChannelList : ServerBasePacket
    {
        public override string GetType()
        {
            return "MSG_CHANNEL_LIST";
        }

        public override byte GetId()
        {
            return 0xd9;
        }

        protected override void WriteImpl()
        {
            // Size
            WriteInt(ChatManager.Channels.Count);
            
            // All of them
            foreach (var kvp in ChatManager.Channels)
            {
                WriteInt(kvp.Key); // Channel Id
                
                WriteString(kvp.Value.Name); // Channel name
            }
        }
    }
}