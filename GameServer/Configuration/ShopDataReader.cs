using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using GameServer.Configuration.Shop;
using Swan.Logging;

namespace GameServer.Configuration
{
    /// <summary>
    /// The helper class to read config files for item pricing
    /// </summary>
    public static class ShopDataReader
    {
        private static readonly Dictionary<uint, Good> _allGoods = new Dictionary<uint, Good>();
        
        // Temporary - store in memory?
        public static List<Good> Set;
        public static List<Good> Weapon;
        public static List<Good> Head;
        public static List<Good> Chest;
        public static List<Good> Arm;
        public static List<Good> Leg;
        public static List<Good> Booster;
        public static List<Good> Code;
        public static List<Good> Etc;

        /// <summary>
        /// Reads the goods data
        /// TODO: Configure path via config file
        /// </summary>
        public static void ReadGoods()
        {
            Set = Read("set");
            Weapon = Read("weapon");
            Head = Read("head");
            Chest = Read("chest");
            Arm = Read("arm");
            Leg = Read("leg");
            Booster = Read("booster");
            Code = Read("code");
            Etc = Read("etc");
        }

        /// <summary>
        /// Finds a good by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Good GetGoodById(uint id)
        {
            return _allGoods[id];
        }

        private static List<Good> Read(string csvName)
        {
            List<Good> data;
            
            $"Reading {csvName}(s)...".Debug();
            
            using (var reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), "Config/Shop/",
                $"{csvName}.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Read weapons
                data = csv.GetRecords<Good>().ToList();
                
                // Add to all list
                data.ForEach(g => _allGoods.Add(g.Id, g));
            }
            $"Done! Read {data.Count} {csvName}(s)!".Debug();

            return data;
        }
    }
}