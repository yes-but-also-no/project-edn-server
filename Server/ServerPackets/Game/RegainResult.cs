using Data.Model;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent when the user sucessfully or unsucessfully respawns i think
    /// </summary>
    public class RegainResult : ServerBasePacket
    {
        private readonly UnitRecord _unit;
        private readonly bool _result;

        public RegainResult(UnitRecord unit, bool result)
        {
            _unit = unit;
            _result = result;
        }
        
        public override string GetType()
        {
            return "GAME_REGAIN_RESULT";
        }

        public override byte GetId()
        {
            return 0x57;
        }

        protected override void WriteImpl()
        {
            WriteInt(_result ? 0 : 1); // Result code?
            WriteInt(_unit.Id); // Unit id?
        }
    }
}