using System;
using System.Drawing;
using System.Linq;
using System.Numerics;
using GameServer.Model.Units;
using GameServer.ServerPackets.Game;
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

            if (Program.IsSimulating)
            {
                GetClient().SendPacket(new UserInfo(null, GetClient().User));
                
                var spawnedUnit = new UserUnit(null, GetClient().User.DefaultUnit, GetClient());
                
                spawnedUnit.WorldPosition = new Vector3(3633,196,-66);

                spawnedUnit.Team = 1;
                
                GetClient().SendPacket(new UnitInfo(spawnedUnit, null));
                
                GetClient().SendPacket(new SpawnUnit(spawnedUnit));
                
                GetClient().SendPacket(new GameStarted());
                return;
            }
            
            client.GameInstance?.OnGameReady(client);
        }
    }
}