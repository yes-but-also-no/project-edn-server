using Data.Model;
using GameServer.Game;

namespace GameServer.Model.Units
{
    public class NpcUnit : Unit
    {
        public NpcUnit(GameInstance instance, UnitRecord unitRecord) : base(instance, unitRecord)
        {
        }
    }
}