using System;
using System.Drawing;
using System.Linq;
using Console = Colorful.Console;

namespace GameServer.ClientPackets.Game
{
    /// <summary>
    /// Sent when a user has sucessfully loaded into the game and is ready to play
    /// </summary>
    public class ReadyGame : ClientBasePacket
    {
        public ReadyGame(byte[] data, GameSession client) : base(data, client)
        {
        }

        public override string GetType()
        {
            return "GAME_READY_GAME";
        }

        protected override void RunImpl()
        {
            var client = GetClient();

            client.GameInstance?.OnGameReady(client);
        }
    }
}