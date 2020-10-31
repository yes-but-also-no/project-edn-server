using System;
using GameServer.ServerPackets.Game;

namespace GameServer.ClientPackets.Tutorial
{
    /// <summary>
    /// Sent when someone starts a tutorial i believe
    /// </summary>
    public class TutorialSelect : ClientBasePacket
    {
        public TutorialSelect(byte[] data, GameSession client) : base(data, client)
        {
            Console.WriteLine("Got tutorial select " + GetInt());
        }

        public override string GetType()
        {
            return "TUTORIAL_SELECT";
        }

        protected override void RunImpl()
        {
            GetClient().SendPacket(new TutorialBegin());
        }
    }
}