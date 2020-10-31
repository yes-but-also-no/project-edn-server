using System.Linq;
using Data;
using Data.Model;

namespace GameServer.ClientPackets.Hangar
{
    /// <summary>
    /// Sent when the user changes the unit name
    /// </summary>
    public class FactoryChangeUnitName : ClientBasePacket
    {
        private readonly UnitRecord _patchUnit = new UnitRecord();
        
        public FactoryChangeUnitName(byte[] data, GameSession client) : base(data, client)
        {
            _patchUnit.Id = GetInt();

            _patchUnit.Name = GetString();
        }

        public override string GetType()
        {
            return "FACTORY_CHANGE_UNITNAME";
        }

        protected override void RunImpl()
        {
            // Patch to database
            using (var db = new ExteelContext())
            {
                var unit = db.Units.Single(u => u.Id == _patchUnit.Id);
                
                // Patch unit
                unit.Name = _patchUnit.Name;

                db.SaveChanges();
            }
            
            GetClient().UpdateUserFromDatabase();
            
            GetClient().SendPacket(new ServerPackets.Hangar.NameIsValid(true));
        }
    }
}