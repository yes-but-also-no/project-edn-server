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

