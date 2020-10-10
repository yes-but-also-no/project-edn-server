using GameServer.Model.Units;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Called when the game wants to spawn a unit
    /// </summary>
    public class SpawnUnit : ServerBasePacket
    {
        private readonly Unit _unit;

        public SpawnUnit(Unit unit)
        {
            _unit = unit;
        }
        
        public override string GetType()
        {
            return "GAME_SPAWN_UNIT";
        }

        public override byte GetId()
        {
            return 0x4d;
        }

        protected override void WriteImpl()
        {
            // TODO: Move this to helper function?
            // TODO: Spawn maps?
            
            WriteInt(_unit.Id);
            WriteInt(0); // Unknown - weapon set?
            
            // TODO: WriteVector3?
            WriteFloat(_unit.WorldPosition.X);
            WriteFloat(_unit.WorldPosition.Y);
            WriteFloat(_unit.WorldPosition.Z);
            
            // TODO: WriteVector2?
            WriteUShort(_unit.AimY);
            WriteUShort(_unit.AimX);
        }
    }
}