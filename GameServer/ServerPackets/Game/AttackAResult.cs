using GameServer.Model.Parts.Weapons;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent to confirm projectile hits? 
    /// </summary>
    public class AttackAResult : ServerBasePacket
    {
        private readonly bool _clear;
        private readonly Weapon _weapon;

        public AttackAResult( Weapon weapon, bool clear)
        {
            _clear = clear;
            _weapon = weapon;
        }
        
        public override string GetType()
        {
            return "ATTACK_A_RESULT";
        }

        public override byte GetId()
        {
            return 0x72;
        }

        protected override void WriteImpl()
        {
            //var targetId = _weapon.Target?.Id ?? 0;
            
            WriteInt(GameServer.RunningMs); // Unknown
            WriteInt(_weapon.Id); // IFO Id?
            
            WriteInt(1); // Array size

            if (_clear) // Clear old projectile
            {
                WriteUInt(0x02000000);
                WriteUInt(0);
                WriteUInt(0);
                return;
            }
            
//            if (targetId != 0)
//                WriteUInt(0x01000000); // Result
//            else
                WriteUInt(0x04000000); // Result
            
            // NOTE: On server, a miss is 0x00 0x00 0x00 0x08?
            
            WriteInt(-1); // Target id
            WriteInt(0); // Damage
            
            // Results look to be:
            // 0x01000000 - Hit
            // 0x02000000 - Unknown
            // 0x30000000 - Block
            // 0x04000000 - Miss / Hit terrain
            // 0x08000000 - Unknown
        }
    }
}