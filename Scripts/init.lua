print("Hello world!")

--local ent = createEntity("ScriptedEntity", "lua_created_entity")
--local ent2 = createEntity("TestEntity", "lua_created_entity2")

-- Global entities manager
--entities = {
--    MyLuaEntity = ENT
--}
--
---- Creator
--entities.create = function(entityClass, entityName)
--    if entities[entityClass] ~= nil then
--        return entities[entityClass](entityName)
--    else
--        return createEntity(entityClass, entityName)
--    end
--end

---

local t = entities.create("TestEntity", "native_ent_1")
local t2 = entities.create("MyLuaEntity", "scripted_ent_1")
--t2.InstanceVar = 9

print(t.InstanceVar)
print(t2.InstanceVar)

print(t.EngineName)
print(t2.EngineName)