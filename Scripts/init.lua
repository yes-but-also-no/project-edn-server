print("Hello world!")

local ent = createEntity("ScriptedEntity", "lua_created_entity")
--local ent2 = createEntity("TestEntity", "lua_created_entity2")

-- Create ENT base class
local ENT = {}
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
                
                -- We search the native
                return t.__native[k]
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

-- SIMULATED USER FILE --

ENT.Base = "ScriptedEntity"
ENT.InstanceVar = 5
ENT.ClassName = "MyLuaEntity"

-- Our initializer hook
function ENT:Initialize()
    print("LUA ENT INITIALIZED!")
    self.InstanceVar = 69;
end

-- Custom function, not in C#
function ENT:MyCustomFunction()
    print(self.EngineName)
end

-- Hooked function, from C# base class
function ENT:OnTick()
    print("I TICKED")
end

-- END SIMULATED USER FILE

-- Global entities manager
entities = {
    MyLuaEntity = ENT
}

-- Creator
entities.create = function(entityClass, entityName)
    if entities[entityClass] ~= nil then
        return entities[entityClass](entityName)
    else
        return createEntity(entityClass, entityName)
    end
end

---

local t = entities.create("TestEntity", "native_ent_1")
local t2 = entities.create("MyLuaEntity", "scripted_ent_1")
--t2.InstanceVar = 9

print(t.InstanceVar)
print(t2.InstanceVar)

print(t.EngineName)
print(t2.EngineName)