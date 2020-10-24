using System.Collections.Generic;
using System.Numerics;
using Data.Model;
using GameServer.Game;
using GameServer.Model.Results;
using GameServer.ServerPackets.Chat;

namespace GameServer.Managers
{
    /// <summary>
    /// Handles admin commands
    /// </summary>
    public static class AdminManager
    {
        /// <summary>
        /// Called when an admin sends a game commands
        /// </summary>
        /// <param name="user"></param>
        /// <param name="game"></param>
        public static void OnGameAdminCommand(GameSession client, List<string> commands)
        {
            switch (commands[0])
                {
                    case "#spawn":
                        client.GameInstance.SpawnNpc(client.CurrentUnit.WorldPosition);
                        break;
                    
                    case "#kill":
                        // TODO: Kill player maybe
                        break;
                    
                    case "#killme":
                        client.GameInstance.KillUnit(client.CurrentUnit);
                        break;
                    
                    case "#where":
                        var unitPos = client.CurrentUnit.WorldPosition;
                        
                        client.SendPacket(new Message(client.User.Callsign, 
                            $"X: {unitPos.X}, Y: {unitPos.Y} Z: {unitPos.Z}"));
                        
                        client.SendPacket(new Message(client.User.Callsign, 
                            $"geoX: {client.GameInstance.Map.GetGeoX((int)unitPos.X)}, geoY: {client.GameInstance.Map.GetGeoY((int)unitPos.Y)}"));
                        break;
                    
                    case "#god":
                        client.CurrentUnit.GodMode = !client.CurrentUnit.GodMode;
                        var msg = client.CurrentUnit.GodMode ? "ENABLED" : "DISABLED";
                        client.GameInstance.BroadcastChat(
                            $"God mode <{msg}> for user {client.User.Callsign}"
                            );
                        break;
                    
                    case "#sp":
                        // Broadcast
                        client.GameInstance.NotifyUnitAttacked(client.CurrentUnit, new HitResult
                        {
                            ResultCode = HitResultCode.Hit,
                            Damage = 2000,
                            PushBack = Vector3.Zero,
                            VictimId = client.CurrentUnit.Id
                        }, client.CurrentUnit.WeaponSetPrimary.Left);
                        
                        break;
                }
        }
    }
}