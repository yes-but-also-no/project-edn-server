using System;
using System.Drawing;
using System.Linq;
using GameServer.ServerPackets.Game;
using Console = Colorful.Console;

namespace GameServer.ClientPackets.Game
{
    /// <summary>
    /// Sent when the user first spawns. Not sure exactly how it works
    /// </summary>
    public class SelectBase : ClientBasePacket
    {
        public SelectBase(byte[] data, GameSession client) : base(data, client)
        {
        }

        public override string GetType()
        {
            return "GAME_SELECT_BASE";
        }

        protected override void RunImpl()
        {
            GetClient().SendPacket(new BaseSelected());
        }
    }
}