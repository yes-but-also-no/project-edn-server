using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using GameServer.Game;
using GameServer.GeoEngine;
using GameServer.Model.Parts.Weapons;
using GameServer.Model.Results;
using GameServer.Model.Units;
using GameServer.Util;
using Console = Colorful.Console;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent when a user performs a melee attack
    /// </summary>
    public class AttackBlade : ServerBasePacket
    {
        private readonly Unit _unit;
        private readonly Weapon _weapon;
        private readonly IEnumerable<HitResult> _hits;

        public AttackBlade(Unit unit, IEnumerable<HitResult> hits, Weapon weapon)
        {
            HighPriority = true;
            
            _unit = unit;
            _weapon = weapon;
            _hits = hits;
        }
        
        public override string GetType()
        {
            return "ATTACK_BLADE";
        }

        public override byte GetId()
        {
            return 0x6d;
        }

        protected override void WriteImpl()
        {
            WriteInt(GameServer.RunningMs); // Game time
            
            WriteRaw(_hits.Select(h => (byte)h.ResultCode).ToArray()); // Result code seems to be flags - 01 02 02 02 is hit miss miss miss?
            
            WriteInt(_unit.Id); // Attacker Id - maybe passed as an index?
            WriteInt((int)_weapon.Arm); // Arm

            // Write results
            foreach (var hit in _hits)
            {
                WriteInt(hit.VictimId);
                WriteInt(hit.Damage);
                
                WriteBool(hit.PushBack != Vector3.Zero); // If push is not zero, it should pushback
                
                WriteFloat(hit.PushBack.X);
                WriteFloat(hit.PushBack.Y);
                WriteFloat(hit.PushBack.Z);
            }

            WriteInt(_weapon.ComboStep); // Combo step
            
            WriteUShort(_unit.AimY); // Attacker - AimX
            WriteUShort(_unit.AimX); // Attacker - AimY
            
            WriteFloat(_unit.WorldPosition.X); // Attacker - X
            WriteFloat(_unit.WorldPosition.Y); // Attacker - Y
            WriteFloat(_unit.WorldPosition.Z); // Attacker - Z
            
            WriteBool(_unit.InAir); // Unknown - air related?

            if (_unit.InAir)
            {
                var degY = _unit.AimY / 182.041666667f;
                
                if (degY > 30.0f && degY < 100.0f) // Aim up
                    WriteInt(1); // Air attack type! 1 - Attack upwards 2 - Attack lateral 3 - Attack downwards
                else if (degY > 200.0f && degY < 330.0f)
                    WriteInt(3);
                else
                    WriteInt(2);
            }
        }
    }
}