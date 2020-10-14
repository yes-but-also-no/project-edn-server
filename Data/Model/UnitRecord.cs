using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.Text;
using Data.Model.Items;

namespace Data.Model
{
    /// <summary>
    /// This class represents a unit in the game
    /// May need to be switched to NPC/User in the future
    /// </summary>
    public class UnitRecord
    {
        /// <summary>
        /// Unique Id for this unit.
        /// Not sure if it is globally unique or just for the same session
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The user who is owns this unit
        /// Not mapped for now as I think we might need to assign this at runtime, for NPC units and stuff
        /// </summary>
        [NotMapped]
        public ExteelUser User { get; set; }
        
        /// <summary>
        /// The inventory this unit is part of
        /// </summary>
        public int UserInventoryId { get; set; }

        /// <summary>
        /// The launch order position of this unit
        /// </summary>
        public int LaunchOrder { get; set; }

        /// <summary>
        /// The name of this unit
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The head for this unit
        /// </summary>
        public PartRecord Head { get; set; }
        public int HeadId { get; set; }
        
        /// <summary>
        /// The chest for this unit
        /// </summary>
        public PartRecord Chest { get; set; }
        public int ChestId { get; set; }
        
        /// <summary>
        /// The arms for this unit
        /// </summary>
        public PartRecord Arms { get; set; }
        public int ArmsId { get; set; }
        
        /// <summary>
        /// The legs for this unit
        /// </summary>
        public PartRecord Legs { get; set; }
        public int LegsId { get; set; }
        
        /// <summary>
        /// The backpack for this unit
        /// </summary>
        public PartRecord Backpack { get; set; }
        public int BackpackId { get; set; }
        
        /// <summary>
        /// The left weapon for this units first weapon set
        /// </summary>
        public PartRecord WeaponSet1Left { get; set; }
        public int? WeaponSet1LeftId { get; set; }
        
        /// <summary>
        /// The right weapon for this units first weapon set
        /// </summary>
        public PartRecord WeaponSet1Right { get; set; }
        public int? WeaponSet1RightId { get; set; }
        
        /// <summary>
        /// The left weapon for this units second weapon set
        /// </summary>
        public PartRecord WeaponSet2Left { get; set; }
        public int? WeaponSet2LeftId { get; set; }
        
        /// <summary>
        /// The right weapon for this units second weapon set
        /// </summary>
        public PartRecord WeaponSet2Right { get; set; }
        public int? WeaponSet2RightId { get; set; }
        
        /// <summary>
        /// Skill 1
        /// </summary>
        public PartRecord Skill1 { get; set; }
        public int? Skill1Id { get; set; }
        
        /// <summary>
        /// Skill 2
        /// </summary>
        public PartRecord Skill2 { get; set; }
        public int? Skill2Id { get; set; }
        
        /// <summary>
        /// Skill 3
        /// </summary>
        public PartRecord Skill3 { get; set; }
        public int? Skill3Id { get; set; }
        
        /// <summary>
        /// Skill 4
        /// </summary>
        public PartRecord Skill4 { get; set; }
        public int? Skill4Id { get; set; }

        /// <summary>
        /// This will generate the matching file name for this units setup, which should be uploaded by the client
        /// </summary>
        public string ImageName
        {
            get
            {
                // Create the base string
                var baseString = new StringBuilder()
                    .AppendFormat("H{0}{1:x2}{2:x2}{3:x2}", Head.TemplateId, Head.Color.R, Head.Color.G, Head.Color.B)
                    .AppendFormat("C{0}{1:x2}{2:x2}{3:x2}", Chest.TemplateId, Chest.Color.R, Chest.Color.G, Chest.Color.B)
                    .AppendFormat("A{0}{1:x2}{2:x2}{3:x2}", Arms.TemplateId, Arms.Color.R, Arms.Color.G, Arms.Color.B)
                    .AppendFormat("L{0}{1:x2}{2:x2}{3:x2}", Legs.TemplateId, Legs.Color.R, Legs.Color.G, Legs.Color.B)
                    .AppendFormat("B{0}{1:x2}{2:x2}{3:x2}", Backpack.TemplateId, Backpack.Color.R, Backpack.Color.G, Backpack.Color.B)
                    .AppendFormat("W{0}{1:x2}{2:x2}{3:x2}", WeaponSet1Left.TemplateId, WeaponSet1Left.Color.R, WeaponSet1Left.Color.G, WeaponSet1Left.Color.B)
                    .AppendFormat("W{0}{1:x2}{2:x2}{3:x2}", WeaponSet1Right.TemplateId, WeaponSet1Right.Color.R, WeaponSet1Right.Color.G, WeaponSet1Right.Color.B)
                    .ToString();
                
                // Convert to bytes
                var bytes = Encoding.UTF8.GetBytes(baseString);
                uint checksum = 0;
                
                // Encode it
                foreach(byte b in bytes) {
                    checksum = checksum * 0x10 + b;
                    var uVar3 = checksum & 0xf0000000;
                    if (uVar3 != 0) {
                        checksum = checksum ^ uVar3 >> 0x18 ^ uVar3;
                    }
                }

                checksum = checksum % 1000;
                
                // TEMP: This is based on the "image upload code" in the clients localization.ini
                const int regionCode = 26;

                // Return the new string
                return $"{regionCode:0000}{checksum:0000}{baseString}.jpg";
            }
        }
    }
}