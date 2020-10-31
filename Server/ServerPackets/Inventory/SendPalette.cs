using Data.Model;

namespace GameServer.ServerPackets.Inventory
{
    /// <summary>
    /// Sends the Palette?? for a unit
    /// Maybe equiped skills?
    /// </summary>
    public class SendPalette : ServerBasePacket
    {
        /// <summary>
        /// The unit that this packet is for
        /// </summary>
        private readonly UnitRecord _unit;

        public SendPalette(UnitRecord unit)
        {
            HighPriority = true;
            
            _unit = unit;
        }
            
        public override string GetType()
        {
            return "INV_SEND_PALETTE";
        }

        public override byte GetId()
        {
            return 0x23;
        }

        protected override void WriteImpl()
        {
            WriteInt(_unit.Id);
            
            WriteInt(4); // Slot count?
            
            // Figure out how many codes?
            // Fuck im drunk
            var count = 0;

            if (_unit.Skill1Id != null)
                count++;
            
            if (_unit.Skill2Id != null)
                count++;
            
            if (_unit.Skill3Id != null)
                count++;
            
            if (_unit.Skill4Id != null)
                count++;
            
            
            WriteInt(count); // Code count?
            
            if (_unit.Skill1Id != null)
                WriteInt(_unit.Skill1Id.Value);
            
            if (_unit.Skill2Id != null)
                WriteInt(_unit.Skill2Id.Value);
            
            if (_unit.Skill3Id != null)
                WriteInt(_unit.Skill3Id.Value);
            
            if (_unit.Skill4Id != null)
                WriteInt(_unit.Skill4Id.Value);
        }
    }
}