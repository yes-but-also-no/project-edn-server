using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
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
        /// Stores the time value the client has sent us. This will be used to calculate cooldowns i think
        /// </summary>
//        private uint _lastTick = 0;

        /// <summary>
        /// Gets the players last tick
        /// </summary>
//        public uint GetLastTick => _lastTick;

        /// <summary>
        /// Called when we get an updated timestamp. Will trickle down tick calls with delta to stuff
        /// TODO: Should this be an abstract class for all items?
        /// </summary>
        /// <param name="timeStamp"></param>
//        public float UpdatePing(uint timeStamp)
//        {
//            // Calculate delta
//            var delta = timeStamp - _lastTick;
//            
//            // Update timestamp
//            _lastTick = timeStamp;
//            
//            // Return delta
//            return delta;
//        }
    }
}