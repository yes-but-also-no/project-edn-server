using System.Collections.Generic;
using System.IO;
using Swan.Logging;

namespace GameServer.GeoEngine
{
    /// <summary>
    /// Handles all map data functions for the game
    /// </summary>
    public static class GeoEngine
    {
        private static Dictionary<string, Map> _maps = new Dictionary<string, Map>();
        
        /// <summary>
        /// Loads all maps into memory. This uses about 200mb of memory lmfao
        /// </summary>
        public static void LoadAllMaps()
        {
            foreach (var fileName in Directory.GetFiles("Config/Maps/", "*.map"))
            {
                _maps.Add(
                    Path.GetFileNameWithoutExtension(fileName).ToLower(),
                    LoadMap(fileName)
                );
            }
        }

        private static Map LoadMap(string fileName)
        {
           $"Reading map {Path.GetFileName(fileName)}...".Debug();

            Map map;

            using (var reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                // Discard bullshit
                reader.ReadBytes(28);
                
                map = new Map(reader);
            }

            return map;
        }
        
        /// <summary>
        /// Returns the map instance for a given map name
        /// </summary>
        /// <param name="mapName"></param>
        /// <returns></returns>
        public static Map GetGeoMap(string mapName)
        {
            return _maps[mapName.ToLower()];
        }
    }
}