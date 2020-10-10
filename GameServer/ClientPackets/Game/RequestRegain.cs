using System;
using System.Drawing;
using System.Linq;
using System.Numerics;
using GameServer.ServerPackets.Game;
using Console = Colorful.Console;

namespace GameServer.ClientPackets.Game
{
    /// <summary>
    /// Called when the user wishes to respawn
    /// </summary>
    public class RequestRegain : ClientBasePacket
    {
        private readonly uint _unitId;
        private readonly uint _baseId;
        
        public RequestRegain(byte[] data, GameSession client) : base(data, client)
        {
            _unitId = GetUInt();
            _baseId = GetUInt();
        }

        public override string GetType()
        {
            return "REQ_REGAIN";
        }

        protected override void RunImpl()
        {
            var client = GetClient();
            var unit = client.User.DefaultUnit;
            
            // Send success
            client.SendPacket(new RegainResult(unit, client.GameInstance.TryRegain(unit, client)));
            
            // Send unit info
            //client.GameInstance.SpawnUnit(unit, client.User);
        }
    }
}