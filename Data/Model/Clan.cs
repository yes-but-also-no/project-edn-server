using System.Collections.Generic;

namespace Data.Model
{
    public class Clan
    {
        /// <summary>
        /// The unique Id of this clan
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The name of this clan
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of users in this clan
        /// </summary>
        public List<ExteelUser> Users { get; set; }

        // Stub: There should be a ref somewher to the clan image
        // Looked to be uploaded via a web sign in, accepting bmp files
        // That was safe, right? lmfao
    }
}