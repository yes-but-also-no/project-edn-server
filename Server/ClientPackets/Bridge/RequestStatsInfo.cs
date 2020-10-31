using System;
using System.Linq;
using Data.Model;
using GameServer.ServerPackets;
using GameServer.ServerPackets.Bridge;
using GameServer.ServerPackets.Bridge.Stats;

namespace GameServer.ClientPackets.Bridge
{
    /// <summary>
    /// When the user requests their stats
    /// </summary>
    public class RequestStatsInfo : ClientBasePacket
    {
        public RequestStatsInfo(byte[] data, GameSession client) : base(data, client)
        {
        }

        public override string GetType()
        {
            return "BRIDGE_REQ_STAT_INFO";
        }

        protected override void RunImpl()
        {
            // Determine the stat type based on the Id offset
            var statType = (UserStats.StatType)(Id - 0x47);
            
            // Lookup stats
            var stats = GetClient().User.Stats.FirstOrDefault(s => s.Type == statType);

            if (stats == null)
            {
                Console.WriteLine("Unable to find stat type of {0} on user!", statType);
                return;
            }

            ServerBasePacket packet;

            switch (statType)
            {
                case UserStats.StatType.Training:
                    packet = new TrainingInfo(stats);
                    break;
                
                case UserStats.StatType.Survival:
                    packet = new SurvivalInfo(stats);
                    break;
                
                case UserStats.StatType.TeamSurvival:
                    packet = new TeamSurvivalInfo(stats);
                    break;
                
                case UserStats.StatType.TeamBattle:
                    packet = new TeamBattleInfo(stats);
                    break;
                
                case UserStats.StatType.Ctf:
                    packet = new CtfInfo(stats);
                    break;
                
                case UserStats.StatType.ClanBattle:
                    packet = new ClanBattleInfo(stats);
                    break;
                
                case UserStats.StatType.DefensiveBattle:
                    packet = new DefensiveBattleInfo(stats);
                    break;
                
                default:
                    return;
            }
            
            GetClient().SendPacket(packet);
        }
    }
}