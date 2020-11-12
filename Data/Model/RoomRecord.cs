using Data.Configuration.Poo;

namespace Data.Model
{
    /// <summary>
    /// Record for a room
    /// THIS IS NOT STORED IN ANY DATABASE
    /// </summary>
    public class RoomRecord
    {
        /// <summary>
        /// Room id
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Room name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Room password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Game template id
        /// </summary>
        public uint TemplateId { get; set; }

        /// <summary>
        /// For LS MODE, game difficulty
        /// </summary>
        public int Difficulty { get; set; }
        
        /// <summary>
        /// Is balance enabled?
        /// </summary>
        public bool Balance { get; set; }
        
        /// <summary>
        /// Is 5 minute mode enabled?
        /// </summary>
        public bool FiveMinute { get; set; }

        /// <summary>
        /// Is SD (Big Head) mode enabled?
        /// </summary>
        public bool SdMode { get; set; }
        
        /// <summary>
        /// Is this room visible on the public list?
        /// </summary>
        public bool Visible { get; set; }
        
        /// <summary>
        /// Max level desired
        /// </summary>
        public int MaxLevel { get; set; }
        
        /// <summary>
        /// Min level desired
        /// </summary>
        public int MinLevel { get; set; }
    }
}