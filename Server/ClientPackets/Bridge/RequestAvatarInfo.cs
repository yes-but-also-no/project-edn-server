using GameServer.ServerPackets.Bridge;

namespace GameServer.ClientPackets.Bridge
{
    /// <summary>
    /// Called when the user visits their main lobby window and we need their account stats and info
    /// </summary>
    public class RequestAvatarInfo : ClientBasePacket
    {
        public RequestAvatarInfo(byte[] data, GameSession client) 
            : base(data, client) {}

        public override string GetType()
        {
            return "BRIDGE_REQ_AVATAR_INFO";
        }

        protected override void RunImpl()
        {
            var client = GetClient();
            client.SendPacket(new AvatarInfo(client.User));
        }
    }
}