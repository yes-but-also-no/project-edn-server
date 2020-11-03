using System;
using System.Collections.Generic;
using System.IO;
using NLua;
using Swan.Logging;

namespace Game.LuaEngine
{
    /// <summary>
    /// This is the lua engine for the game
    /// It will handle loading of user scripts and making them available to c#
    /// It has access to a global lua scope, as well as instanced lua scopes per game
    /// </summary>
    public static class LuaEngine
    {
        /// <summary>
        /// TODO: Move to configuration
        /// </summary>
        private static readonly string LuaPath = Path.Combine(Directory.GetCurrentDirectory(), "Scripts/");

        /// <summary>
        /// Loads all lua entities, making them available for the engine
        /// </summary>
        public static void LoadAllEntities(Lua lua)
        {
            // Create global entities table
            AddEntityBindings(lua);
            
            // Loop all entities
            foreach (var fileName in Directory.GetFiles(Path.Combine(LuaPath, "Entities")))
            {
                // Load it
                LoadEntity(lua, fileName);
            }
        }
        
        /// <summary>
        /// This will parse a single custom lua entity
        /// </summary>
        private static void LoadEntity(Lua lua, string entityFileName)
        {
            // Create the table
            lua.NewTable("ENT");
            
            // Sandboxed entity header
            lua.DoFile(Path.Combine(LuaPath, "Internal/entity_template.lua"));
            
            // Load custom code
            lua.DoFile(entityFileName);

            // Export it
            var something = lua["ENT"] as LuaTable;
            
            // TODO: More error checks
            var className = something?["ClassName"] as string;

            if (string.IsNullOrEmpty(className))
                $"Unable to load custom entity from {entityFileName}! ENT does not contain a ClassName!".Warn();
            else
            {
                // Grab table ref
                var entities = lua["entities"] as LuaTable;
                
                // Add to global dict
                entities[className] = something;
                
                // Log
                $"Loaded custom lua entity {className}!".Info();
            }
        }
        
        /// <summary>
        /// Adds all custom entity bindings to a lua state
        /// </summary>
        /// <returns></returns>
        private static void AddEntityBindings(Lua lua)
        {
            // Create table
            lua.NewTable("entities");
            
            // Create functions
            lua.DoFile(Path.Combine(LuaPath, "Internal/g_entities.lua"));
        }
    }
}