using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using GameServer.Configuration.Game;
using GameServer.Configuration.Shop;
using Swan.Logging;

namespace GameServer.Configuration
{
    /// <summary>
    /// The helper class to read config files for item pricing
    /// </summary>
    public static class SpawnDataReader
    {
        private static readonly Dictionary<string, MapSpawnInfo> _allSpawns = new Dictionary<string, MapSpawnInfo>();
        
        private static readonly MapSpawnInfo DummySpawn = new MapSpawnInfo
        {
            PlayerSpawns = new Dictionary<int, Spawn>
            {
                {0, new Spawn{Id = 0, X = 0, Y = 0, Z = 2000}}
            }
        };

        /// <summary>
        /// Loads all spawns into memory.
        /// </summary>
        public static void LoadAllSpawns()
        {
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Config/Spawns/")) 
                || Directory.GetFiles("Config/Spawns/", "*.spn").Length < 1)
            {
                throw new FileNotFoundException("Spawn maps (*.spn) are missing!");
            }
            
            foreach (var fileName in Directory.GetFiles("Config/Spawns/", "*.spn"))
            {
                _allSpawns.Add(
                    Path.GetFileNameWithoutExtension(fileName).ToLower(),
                    Read(fileName)
                );
            }
        }

        /// <summary>
        /// Finds spawn info by map name
        /// </summary>
        /// <param name="mapName"></param>
        /// <returns></returns>
        public static MapSpawnInfo GetSpawnInfoForMap(string mapName)
        {
            var name = mapName.ToLower();
            
            return _allSpawns.ContainsKey(name) ? _allSpawns[name] : DummySpawn;
        }

        private static MapSpawnInfo Read(string csvName)
        {
            var spawnInfo = new MapSpawnInfo {PlayerSpawns = new Dictionary<int, Spawn>()};


            $"Reading {csvName}...".Debug();
            
            using (var reader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(),
                $"{csvName}")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.Comment = ';';
                csv.Configuration.AllowComments = true;
                
                // Read weapons
                var data = csv.GetRecords<Spawn>().ToList();
                
                // Add to all list
                data.ForEach(g => spawnInfo.PlayerSpawns.Add(g.Id, g));
                
                $"Done! Read {data.Count} spawns from {csvName}!".Debug();
            }
            

            return spawnInfo;
        }
    }
}