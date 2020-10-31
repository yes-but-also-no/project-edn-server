using System;
using System.Drawing;
using System.Linq;
using GameServer.Model.Units;
using GameServer.ServerPackets.Game;
using Swan.Logging;
using Console = Colorful.Console;

namespace GameServer.ClientPackets.Game
{
    /// <summary>
    /// Sent when a user trys to use a skill
    /// </summary>
    public class UseSkill : ClientGameBasePacket
    {
        /// <summary>
        /// The skill used
        /// </summary>
        private readonly int _skillId;

        /// <summary>
        /// Who we hit
        /// </summary>
        private readonly int[] _targets;

        public UseSkill(byte[] data, GameSession client) : base(data, client)
        {
            if (GetClient().GameInstance == null) return;

            var sp = GetInt(); // Current SP
            _skillId = GetInt(); // Skill Id
            
            var num = GetInt(); // Num Enemies hit
            _targets = new int[num]; // Start at a miss

            for (var i = 0; i < num; i++)
            {
                // Temp: just take last
                _targets[i] = GetInt(); // Target id - or self for aoe?
            }

            // High priority
            HighPriority = true;
            
            if (Unit == null) return;

            Unit.CurrentSp = sp;
        }

        public override string GetType()
        {
            return "USE_SKILL";
        }

        protected override void RunImpl()
        {
            // TODO: Miss skill packet
            if (_targets.Length > 0)
                Unit?.TryUseSkill(_skillId, _targets);
        }
    }
}