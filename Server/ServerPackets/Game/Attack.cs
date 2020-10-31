using System.Collections.Generic;
using System.Linq;
using GameServer.Model.Parts.Weapons;
using GameServer.Model.Results;
using GameServer.Model.Units;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Testing this - 69 (nice) seems to be single target attacks
    /// </summary>
    public class Attack : ServerBasePacket
    {
        private readonly Unit _attacker;
        private readonly IEnumerable<HitResult> _hits;
        private readonly Weapon _weapon;

        public Attack(Unit attacker, IEnumerable<HitResult> hits, Weapon weapon)
        {
            _attacker = attacker;
            _hits = hits;
            _weapon = weapon;
        }
        
        public override string GetType()
        {
            return "ATTACK";
        }

        public override byte GetId()
        {
            return 0x67;
        }

        protected override void WriteImpl()
        {  
            WriteInt(GameServer.RunningMs); // Server time

            // TODO: Handle multiple results
            // TODO: Handle miss / block / heal / crit
            
            // TEMP: Force this to act as a single target attack
            var hit = _hits.FirstOrDefault();
            
            WriteRaw(_hits.Select(h => (byte)h.ResultCode).ToArray()); // Result code seems to be flags - 01 02 02 02 is hit miss miss miss?
            
            WriteInt(_attacker.Id); // Attacker Id - maybe passed as an index?
            WriteInt((int)_weapon.Arm); // Arm?
            
            WriteInt(hit.VictimId); // Victim id?
            WriteInt(hit.Damage); // Damage
            
            // If packet type of 0x62 -
            WriteInt(-1); // Unknown
            WriteInt(0); // Unknown
            
            // If packet type of 0x69
//            WriteUInt(targetId); // MultiAttack?
//            WriteInt(_weapon.Damage); // MultiAttack?
//            WriteUInt(targetId); // MultiAttack?
//            WriteInt(_weapon.Damage); // MultiAttack?
//            WriteUInt(targetId); // MultiAttack?
//            WriteInt(_weapon.Damage); // MultiAttack?
            
//            WriteUInt(_unit.Id); // Unknown
//            WriteInt(10); // Unknown
//            WriteUInt(_unit.Id); // Unknown
//            WriteInt(10); // Unknown
            
            WriteUShort(_attacker.AimY); // Attacker - AimX
            WriteUShort(_attacker.AimX); // Attacker - AimY
            
            WriteFloat(_attacker.WorldPosition.X); // Attacker - X
            WriteFloat(_attacker.WorldPosition.Y); // Attacker - Y
            WriteFloat(_attacker.WorldPosition.Z); // Attacker - Z
            
            WriteFloat(_weapon.CurrentOverheat); // Overheat, yess
        }
    }
}