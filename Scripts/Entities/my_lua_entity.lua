-- LUA ENTITY --

-- The base this entity will derive from
-- Must be a C# class
-- Lua derivations will come later
-- 
-- Available values are:
-- ScriptedEntity : Generic scripted entity
-- ScriptedWeapon : Generic scripted weapon
-- ScriptedSkill : Generic scripted skill
-- ScriptedNpc : Generic scripted npc
ENT.Base = "ScriptedEntity"

-- The class name is what you will use to instantiate this entity
ENT.ClassName = "MyLuaEntity"

-- OPTIONAL: You may define any additional optional fields to be prefilled on your entity instances
ENT.InstanceVar = 5

-- OPTIONAL: You may define any additional custom functions for your entities
function ENT:MyCustomFunction()
    print(self.EngineName)
end

-----------
-- HOOKS --
-----------

-- Called as soon as an entity is initialized
function ENT:Initialize()
    print(self:GetEngineId() .. " was initialized")
    
    -- Configure instance vars
    self.InstanceVar = 69
    
    -- Access C# properties
    self:SetTickEnabled(true)
end

-- Called every tick. Will not be called if self.TickEnabled is not set to true
-- Set self.NextTick to a value to delay ticks, otherwise tick will run every frame
function ENT:OnTick(deltaTime)
    print(self:GetEngineId() .. " was ticked " .. deltaTime)
    
    -- Access globals, which are scoped to this game instance
    self:SetNextTick(engine.EngineTime + 1000)
end

-- END SIMULATED USER FILE