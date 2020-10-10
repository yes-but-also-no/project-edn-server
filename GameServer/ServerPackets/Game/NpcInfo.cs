using GameServer.Model.Units;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent in game with data about users units
    /// </summary>
    public class NpcInfo : ServerBasePacket
    {
        private readonly Unit _unit;
        private readonly uint _type;
        private readonly float _scale;

        public NpcInfo(Unit unit, uint type, float scale)
        {
            _unit = unit;
            _type = type;
            _scale = scale;
        }
        
        public override string GetType()
        {
            return "GAME_NPC_INFO";
        }

        public override byte GetId()
        {
            return 0x75;
        }

        protected override void WriteImpl()
        {
            WriteInt(GameServer.RunningMs);
            WriteInt(_unit.Id);
            
            WriteUInt(_unit.Team); // Team
            WriteInt(_unit.CurrentHealth); // Unit HP?
            WriteInt(_unit.CurrentHealth); // Unit HP?
            
            WriteString(_unit.Name);
            
            this.WritePartInfo(_unit.Head);
            this.WritePartInfo(_unit.Chest);
            this.WritePartInfo(_unit.Arms);
            this.WritePartInfo(_unit.Legs);
            this.WritePartInfo(_unit.Booster);
            
            this.WritePartInfo(_unit.WeaponSetPrimary.Left);
            this.WritePartInfo(_unit.WeaponSetPrimary.Right);
            
            this.WritePartInfo(_unit.WeaponSetSecondary.Left);
            this.WritePartInfo(_unit.WeaponSetSecondary.Right);

            WriteUInt(_type);
            WriteFloat(_scale);
//            WriteInt(0);
            WriteInt(0);
            WriteInt(0);
            
//            WriteFloat(_unit.WorldPosition.X);
//            WriteFloat(_unit.WorldPosition.Y);
//            WriteFloat(_unit.WorldPosition.Z);
//            
//            WriteUShort(_unit.AimY);
//            WriteUShort(_unit.AimX);
        }
    }
}