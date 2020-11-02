using System.IO;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

namespace Game
{
    /// <summary>
    /// This handles all script loading for the game
    /// </summary>
    public class GameScriptLoader : ScriptLoaderBase
    {
        /// <summary>
        /// Checks if the script files exist
        /// TODO: Make scripts path configurable
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override bool ScriptFileExists(string name)
        {
            return File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Scripts", name));
        }

        /// <summary>
        /// Reads a file into memory
        /// </summary>
        /// <param name="file"></param>
        /// <param name="globalContext"></param>
        /// <returns></returns>
        public override object LoadFile(string file, Table globalContext)
        {
            return File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Scripts", file));
        }
    }
}