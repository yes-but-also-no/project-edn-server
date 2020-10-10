using GameServer.Model.Units;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Looks to update unit status, maybe for HP or death
    /// </summary>
    public class StatusChanged : ServerBasePacket
    {
        private readonly Unit _unit;
        private readonly bool _allowAttacks;
        private readonly bool _allowDamage;
        private readonly bool _allowMovement;

        public StatusChanged(Unit unit, bool allowAttacks, bool allowDamage, bool allowMovement)
        {
            _unit = unit;

            _allowAttacks = allowAttacks;
            _allowDamage = allowDamage;
            _allowMovement = allowMovement;
        }
        
        public override string GetType()
        {
            return "STATUS_CHANGED";
        }

        public override byte GetId()
        {
            return 0x7a;
        }

        protected override void WriteImpl()
        {
            WriteInt(_unit.Id); // UserId
            WriteInt(_allowAttacks ? 1 : 0); // Can attack? 0 = no 1 = yes?
            WriteInt(_allowDamage ? 1 : 0); // If zero, sheild effect, if 1, not?
            WriteInt(_allowMovement ? 1 : 0); // Can move? if 0 cant, if 1 can?
        }
    }
}