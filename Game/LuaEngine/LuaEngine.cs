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
        /// All custom loaded entities
        /// </summary>
        private static Dictionary<string, LuaTable> _entities = new Dictionary<string, LuaTable>();

        /// <summary>
        /// Adds all custom entity bindings to a lua state
        /// </summary>
        /// <returns></returns>
        public static void AddEntityBindings(Lua lua)
        {
            // Create functions
            lua.DoString(@"
-- Create table
entities = {}

                -- Creator
entities.create = function(entityClass, entityName)
    if entities[entityClass] ~= nil then
        return entities[entityClass](entityName)
    else
        return createEntity(entityClass, entityName)
    end
end
            ");
            
            // Add custom entities
            foreach (var kvp in _entities)
            {
                var ents = lua["entities"] as LuaTable;
                ents[kvp.Key] = kvp.Value;
            }
        }
        
        /// <summary>
        /// Loads all lua entities, making them available for the engine
        /// </summary>
        public static void LoadAllEntities(Lua lua)
        {
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
            // Create a new state
            //using var state = new Lua();
            
            // Sandboxed entity header
            lua.DoString(@"-- Create ENT base class
ENT = {}
ENT.__index = ENT

-- Set meta table for constructor
setmetatable(ENT, {
    -- On call it will scope a new meta table that points to the specific instance native object
    __call = function(cls, ...)
        local self = setmetatable({}, {
            __index = function(t, k)
                -- First we search the instance, if its not there...
                if cls[k] then
                    return cls[k]
                end

                local native = t.__native;

                -- If function
                if type(native[k] == 'function') and getmetatable(native[k]).__call ~= nil then
                    return function(_, ...)
                        return native[k](native, ...)
                    end
                end

                -- We search the native
                return native[k]
            end
        })
        self:_init(...)
        return self
    end
})

-- Instance constructor
function ENT:_init(engineName)
    -- Create native entity
    self.__native = createEntity(self.Base, engineName)

    -- Update the engine name
    self.__native.EngineClass = self.ClassName

    -- Call lua initializer
    self:Initialize()
end

");
            // Load custom code
            lua.DoFile(entityFileName);

            // Export it
            var something = lua.DoString(@"return ENT")[0] as LuaTable;
            
            // TODO: More error checks
            var className = something?["ClassName"] as string;

            if (string.IsNullOrEmpty(className))
                $"Unable to load custom entity from {entityFileName}! ENT does not contain a ClassName!".Warn();
            else
            {
                // Add to global dict
                _entities[className] = something;
                
                // Log
                $"Loaded custom lua entity {className}!".Info();
            }
        }
    }
}