using System;
using GameServer.Model.Parts.Weapons;
using GameServer.Model.Units;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// IFO (Projectile) attacks
    /// </summary>
    public class AttackIfo : ServerBasePacket
    {
        private readonly Unit _unit;
        private readonly Weapon _weapon;
        private readonly Ifo _ifo;

        public AttackIfo(Unit unit, Weapon weapon, Ifo ifo)
        {
            _unit = unit;
            _weapon = weapon;
            _ifo = ifo;
        }
        
        public override string GetType()
        {
            return "ATTACK_IFO";
        }

        public override byte GetId()
        {
            return 0x6f;
        }

        protected override void WriteImpl()
        {
            // TODO: Create object for IFO's
            //Console.WriteLine("IFO TYPE: {0}", _weapon.IfoType);
            
            WriteInt(GameServer.RunningMs); // Time?
            WriteInt(1); // Result code - For hit, crit, sheild
            WriteInt(_unit.Id); // Attacker Id - maybe passed as an index?
            WriteInt((int)_weapon.Arm); // Arm?
            WriteInt(_ifo.Id); // IFO Id
            WriteInt(-1); // Unknown - Damage? -1 on packet
            
            WriteUShort(_unit.AimY); // Attacker - AimX
            WriteUShort(_unit.AimX); // Attacker - AimY
            
            WriteFloat(_unit.WorldPosition.X); // Attacker - X
            WriteFloat(_unit.WorldPosition.Y); // Attacker - Y
            WriteFloat(_unit.WorldPosition.Z); // Attacker - Z
            
            WriteFloat(_weapon.CurrentOverheat); // Overheat, yess
        }
    }
}