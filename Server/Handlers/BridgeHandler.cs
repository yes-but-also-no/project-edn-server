using GameServer.New;
using Network;
using Network.Packets.Client;
using Network.Packets.Client.Bridge;
using Network.Packets.Server.Bridge;

namespace GameServer.Handlers
{
    /// <summary>
    /// This class handles all packets related to bridge, stats, etc
    /// </summary>
    public static class BridgeHandler
    {
        /// <summary>
        /// Called when the client would like to get their avatar info
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(ClientPacketType.RequestAvatarInfo)]
        public static void OnRequestAvatarInfo(GameClient client, RequestAvatarInfoPacket packet)
        {
            // Send response packet
            client.Send(PacketFactory.CreatePacket(new SendAvatarInfoPacket
            {
                User = client.User
            }));
        }
        
        /// <summary>
        /// Called when the client would like to get their stats info
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(ClientPacketType.RequestBestInfo)]
        public static void OnRequestBestInfo(GameClient client, RequestBestInfoPacket packet)
        {
            // Send response packet
            client.Send(PacketFactory.CreatePacket(new SendBestInfoPacket
            {
                User = client.User
            }));
        }
        
        #region STATS
        
        /// <summary>
        /// Called when the client would like to get their training stats info
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(ClientPacketType.RequestStatsTraining)]
        public static void OnRequestStatsTraining(GameClient client, RequestStatsTrainingPacket packet)
        {
            // Send response packet
            client.Send(PacketFactory.CreatePacket(new SendStatsTrainingPacket
            {
                User = client.User
            }));
        }
        
        /// <summary>
        /// Called when the client would like to get their survival stats info
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(ClientPacketType.RequestStatsSurvival)]
        public static void OnRequestStatsSurvival(GameClient client, RequestStatsSurvivalPacket packet)
        {
            // Send response packet
            client.Send(PacketFactory.CreatePacket(new SendStatsSurvivalPacket
            {
                User = client.User
            }));
        }
        
        /// <summary>
        /// Called when the client would like to get their teamSurvival stats info
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(ClientPacketType.RequestStatsTeamSurvival)]
        public static void OnRequestStatsTeamSurvival(GameClient client, RequestStatsTeamSurvivalPacket packet)
        {
            // Send response packet
            client.Send(PacketFactory.CreatePacket(new SendStatsTeamSurvivalPacket
            {
                User = client.User
            }));
        }
        
        /// <summary>
        /// Called when the client would like to get their teamBattle stats info
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(ClientPacketType.RequestStatsTeamBattle)]
        public static void OnRequestStatsTeamBattle(GameClient client, RequestStatsTeamBattlePacket packet)
        {
            // Send response packet
            client.Send(PacketFactory.CreatePacket(new SendStatsTeamBattlePacket
            {
                User = client.User
            }));
        }
        
        /// <summary>
        /// Called when the client would like to get their ctf stats info
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(ClientPacketType.RequestStatsCtf)]
        public static void OnRequestStatsCtf(GameClient client, RequestStatsCtfPacket packet)
        {
            // Send response packet
            client.Send(PacketFactory.CreatePacket(new SendStatsCtfPacket
            {
                User = client.User
            }));
        }
        
        /// <summary>
        /// Called when the client would like to get their clanBattle stats info
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(ClientPacketType.RequestStatsClanBattle)]
        public static void OnRequestStatsClanBattle(GameClient client, RequestStatsClanBattlePacket packet)
        {
            // Send response packet
            client.Send(PacketFactory.CreatePacket(new SendStatsClanBattlePacket
            {
                User = client.User
            }));
        }
        
        /// <summary>
        /// Called when the client would like to get their defensiveBattle stats info
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(ClientPacketType.RequestStatsDefensiveBattle)]
        public static void OnRequestStatsDefensiveBattle(GameClient client, RequestStatsDefensiveBattlePacket packet)
        {
            // Send response packet
            client.Send(PacketFactory.CreatePacket(new SendStatsDefensiveBattlePacket
            {
                User = client.User
            }));
        }
        
        #endregion
    }
}