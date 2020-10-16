using System;
using System.Drawing;
using System.Linq;
using Data;
using Data.Model;
using Data.Model.Items;
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
        
        /// <summary>
        /// Temp parts for patching
        /// </summary>
        private readonly PartRecord _patchHead = new PartRecord();
        private readonly PartRecord _patchChest = new PartRecord();
        private readonly PartRecord _patchArms = new PartRecord();
        private readonly PartRecord _patchLegs = new PartRecord();
        private readonly PartRecord _patchBackpack = new PartRecord();
        
        private readonly PartRecord _patchWeaponSet1Left = new PartRecord();
        private readonly PartRecord _patchWeaponSet1Right = new PartRecord();
        
        private readonly PartRecord _patchWeaponSet2Left = new PartRecord();
        private readonly PartRecord _patchWeaponSet2Right = new PartRecord();
        
        public FactoryModifyUnit(byte[] data, GameSession client) : base(data, client)
        {
//            debug = true;
            
            // Read unit id
            _patchUnit.Id = GetInt();
            
            // Read name
            _patchUnit.Name = GetString();
            
            // Get part Ids
            (_patchUnit.HeadId, _patchHead.Color) = GetPartId();
            (_patchUnit.ChestId, _patchChest.Color) = GetPartId();
            (_patchUnit.ArmsId, _patchArms.Color) = GetPartId();
            (_patchUnit.LegsId, _patchLegs.Color) = GetPartId();
            (_patchUnit.BackpackId, _patchBackpack.Color) = GetPartId();

            (_patchUnit.WeaponSet1LeftId, _patchWeaponSet1Left.Color) = GetPartId();
            (_patchUnit.WeaponSet1RightId, _patchWeaponSet1Right.Color) = GetPartId();

            (_patchUnit.WeaponSet2LeftId, _patchWeaponSet2Left.Color) = GetPartId();
            (_patchUnit.WeaponSet2RightId, _patchWeaponSet2Right.Color) = GetPartId();
            
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
                
                // Patch parts
                var head = db.Parts.Single(p => p.Id == _patchUnit.HeadId);
                head.Color = _patchHead.Color;
                
                var chest = db.Parts.Single(p => p.Id == _patchUnit.ChestId);
                chest.Color = _patchChest.Color;
                
                var arms = db.Parts.Single(p => p.Id == _patchUnit.ArmsId);
                arms.Color = _patchArms.Color;
                
                var legs = db.Parts.Single(p => p.Id == _patchUnit.LegsId);
                legs.Color = _patchLegs.Color;
                
                var backpack = db.Parts.Single(p => p.Id == _patchUnit.BackpackId);
                backpack.Color = _patchBackpack.Color;
                
                // Weapons
                
                var weaponSet1Left = db.Parts.Single(p => p.Id == _patchUnit.WeaponSet1LeftId);
                weaponSet1Left.Color = _patchWeaponSet1Left.Color;
                
                var weaponSet1Right = db.Parts.Single(p => p.Id == _patchUnit.WeaponSet1RightId);
                weaponSet1Right.Color = _patchWeaponSet1Right.Color;
                
                var weaponSet2Left = db.Parts.Single(p => p.Id == _patchUnit.WeaponSet2LeftId);
                weaponSet2Left.Color = _patchWeaponSet2Left.Color;
                
                var weaponSet2Right = db.Parts.Single(p => p.Id == _patchUnit.WeaponSet2RightId);
                weaponSet2Right.Color = _patchWeaponSet2Right.Color;

                db.SaveChanges();
            }
            
            GetClient().UpdateUserFromDatabase();
        }

        /// <summary>
        /// Temp to pull just the ID for now
        /// </summary>
        /// <returns></returns>
        private (int, Color) GetPartId()
        {
            var id = GetInt();

            GetUInt(); // Template Id

            GetUShort(); // Unknown?
            GetUShort(); // Unknown?

            // Color is now pulled in here
            var col = Color.FromArgb(GetByte(), GetByte(), GetByte());
            
            // GetByte(); // R
            // GetByte(); // G
            // GetByte(); // B
            
            GetByte(); // Unknown
            GetByte(); // Unknown
            
            GetInt(); // Unknown
            GetInt(); // Unknown
            
            GetInt(); // Unknown
            GetInt(); // Unknown
            GetInt(); // Unknown
            GetInt(); // Unknown

            return (id, col);
        }
    }
}