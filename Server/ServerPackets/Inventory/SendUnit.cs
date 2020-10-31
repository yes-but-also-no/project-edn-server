using Data.Model;

namespace GameServer.ServerPackets.Inventory
{
    /// <summary>
    /// Sends a single units info packet
    /// </summary>
    public class SendUnit : ServerBasePacket
    {
        /// <summary>
        /// The unit to send
        /// </summary>
        private readonly UnitRecord _unit;

        /// <summary>
        /// The user who owns this unit
        /// </summary>
        private readonly ExteelUser _user;
        
        public SendUnit(ExteelUser user, UnitRecord unit)
        {
            HighPriority = true;
            
            _unit = unit;
            _user = user;
        }

        public override string GetType()
        {
            return "INV_SEND_UNIT_INFO";
        }

        public override byte GetId()
        {
            return 0x22;
        }

        protected override void WriteImpl()
        {
            WriteInt(_user.Id);
            WriteInt(_unit.Id);
            
            WriteString(_unit.Name);
            
            WriteInt(0); // Slot position (1st, 2nd, etc)
            WriteInt(2000); // Unknown - Unit HP?
            
            this.WritePartInfo(_unit.Head);
            this.WritePartInfo(_unit.Chest);
            this.WritePartInfo(_unit.Arms);
            this.WritePartInfo(_unit.Legs);
            this.WritePartInfo(_unit.Backpack);
            
            this.WritePartInfo(_unit.WeaponSet1Left);
            this.WritePartInfo(_unit.WeaponSet1Right);
            
            this.WritePartInfo(_unit.WeaponSet2Left);
            this.WritePartInfo(_unit.WeaponSet2Right);
        }
    }
}