using System;
using System.Linq;
using Data;
using Data.Model;
using Console = Colorful.Console;

namespace GameServer.ClientPackets.Hangar
{
    /// <summary>
    /// Sent when the user hits save on their unit
    /// There is a lot of data here, we will only take a few things for now
    /// TODO: Parse and handle it all
    /// </summary>
    public class FactoryModifyUnit : ClientBasePacket
    {
        /// <summary>
        /// A temp unit to store data
        /// </summary>
        private readonly UnitRecord _patchUnit = new UnitRecord();
        
        public FactoryModifyUnit(byte[] data, GameSession client) : base(data, client)
        {
//            debug = true;
            
            // Read unit id
            _patchUnit.Id = GetInt();
            
            // Read name
            _patchUnit.Name = GetString();
            
            // Get part Ids
            _patchUnit.HeadId = GetPartId();
            _patchUnit.ChestId = GetPartId();
            _patchUnit.ArmsId = GetPartId();
            _patchUnit.LegsId = GetPartId();
            _patchUnit.BackpackId = GetPartId();
            
            _patchUnit.WeaponSet1LeftId = GetPartId();
            _patchUnit.WeaponSet1RightId = GetPartId();
            
            _patchUnit.WeaponSet2LeftId = GetPartId();
            _patchUnit.WeaponSet2RightId = GetPartId();
            
            // Get skills count
            var skillsCount = GetInt();

            // Skill check
            if (skillsCount > 0)
                _patchUnit.Skill1Id = GetInt();
            
            if (skillsCount > 1)
                _patchUnit.Skill2Id = GetInt();
            
            if (skillsCount > 2)
                _patchUnit.Skill3Id = GetInt();
            
            if (skillsCount > 3)
                _patchUnit.Skill4Id = GetInt();
        }

        public override string GetType()
        {
            return "FACTORY_MODIFY_UNIT";
        }

        protected override void RunImpl()
        {
            // Patch to database
            using (var db = new ExteelContext())
            {
                var unit = db.Units.Single(u => u.Id == _patchUnit.Id);
                
                // Patch unit
                unit.Name = _patchUnit.Name;

                unit.HeadId = _patchUnit.HeadId;
                unit.ChestId = _patchUnit.ChestId;
                unit.ArmsId = _patchUnit.ArmsId;
                unit.LegsId = _patchUnit.LegsId;
                unit.BackpackId = _patchUnit.BackpackId;
                
                unit.WeaponSet1LeftId = _patchUnit.WeaponSet1LeftId;
                if (_patchUnit.WeaponSet1RightId != -1)
                    unit.WeaponSet1RightId = _patchUnit.WeaponSet1RightId;
                else
                    unit.WeaponSet1RightId = null;
                
                unit.WeaponSet2LeftId = _patchUnit.WeaponSet2LeftId;
                if (_patchUnit.WeaponSet2RightId != -1)
                    unit.WeaponSet2RightId = _patchUnit.WeaponSet2RightId;
                else
                    unit.WeaponSet2RightId = null;

                unit.Skill1Id = _patchUnit.Skill1Id;
                unit.Skill2Id = _patchUnit.Skill2Id;
                unit.Skill3Id = _patchUnit.Skill3Id;
                unit.Skill4Id = _patchUnit.Skill4Id;

                db.SaveChanges();
            }
            
            GetClient().UpdateUserFromDatabase();
        }

        /// <summary>
        /// Temp to pull just the ID for now
        /// </summary>
        /// <returns></returns>
        private int GetPartId()
        {
            var id = GetInt();

            GetUInt(); // Template Id

            GetUShort(); // Unknown?
            GetUShort(); // Unknown?

            GetByte(); // R
            GetByte(); // G
            GetByte(); // B
            
            GetByte(); // Unknown
            GetByte(); // Unknown
            
            GetInt(); // Unknown
            GetInt(); // Unknown
            
            GetInt(); // Unknown
            GetInt(); // Unknown
            GetInt(); // Unknown
            GetInt(); // Unknown

            return id;
        }
    }
}