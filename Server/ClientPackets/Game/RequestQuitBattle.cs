using GameServer.ServerPackets.Game;
using Console = Colorful.Console;

namespace GameServer.ClientPackets.Game
{
    public class RequestQuitBattle : ClientBasePacket
    {
        /// <summary>
        /// Sent when the user wants to quit the battle they are in
        /// </summary>
        /// <param name="data"></param>
        /// <param name="client"></param>
        public RequestQuitBattle(byte[] data, GameSession client) : base(data, client)
        {
        }

        public override string GetType()
        {
            return "REQ_QUIT_BATTLE";
        }

        protected override void RunImpl()
        {
            var client = GetClient();

            if (client.GameInstance == null)
            {
                Console.WriteLine($"Error! Client {client.GetUserName()} tried to quit battle when not in a battle!");
                return;
            }
            
            // Quit the game
            client.GameInstance.RoomInstance.TryLeaveGame(client);
            
            // Send data to user
            client.SendPacket(new QuitBattle());
        }
    }
}