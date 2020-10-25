using System.Collections.Generic;
using System.Linq;
using GameServer.Model.Parts.Weapons;
using GameServer.Model.Results;
using GameServer.Model.Units;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent to confirm projectile hits? 
    /// </summary>
    public class AttackIfoResult : ServerBasePacket
    {
        private readonly Ifo _ifo;
        private readonly IEnumerable<HitResult> _hits;

        public AttackIfoResult( Ifo ifo, IEnumerable<HitResult> hits)
        {
            _ifo = ifo;
            _hits = hits;
        }
        
        public override string GetType()
        {
            return "ATTACK_IFO_RESULT";
        }

        public override byte GetId()
        {
            return 0x72;
        }

        protected override void WriteImpl()
        {
            //var targetId = _weapon.Target?.Id ?? 0;
            
            WriteInt(GameServer.RunningMs); // Unknown
            WriteInt(_ifo.Id); // IFO Id?

            WriteInt(_hits.Count()); // Array size

            foreach (var hit in _hits)
            {
                WriteRaw(new byte[]{0x00, 0x00, 0x00, (byte)hit.ResultCode});
                WriteInt(hit.VictimId);
                WriteInt(hit.Damage);
            }
            
            // Results look to be:
            // 0x01000000 - Hit
            // 0x02000000 - Unknown
            // 0x30000000 - Block
            // 0x04000000 - Miss / Hit terrain
            // 0x08000000 - Unknown
        }
    }
}