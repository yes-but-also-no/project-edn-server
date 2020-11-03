-- This template file will have ENT pre-seeded
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

                -- Dump if non existing
                if native[k] == nil then
                    return
                end
                
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
    
    -- Bind delegates
    if self.OnSpawn ~= nil then
        self.__native.LuaOnSpawn:Add(function(delta) self:OnSpawn() end)
    end

    if self.OnDeSpawn ~= nil then
        self.__native.LuaOnDeSpawn:Add(function(delta) self:OnDeSpawn(delta) end)
    end

    if self.OnTick ~= nil then
        self.__native.LuaTick:Add(function(delta) self:OnTick(delta) end)
    end

    if self.OnRemoved ~= nil then
        self.__native.LuaOnRemoved:Add(function(delta) self:OnRemoved(delta) end)
    end

    -- Call lua initializer
    self:OnInitialize()
end

