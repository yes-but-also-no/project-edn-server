-- This file initializes the global entities table

-- Creator
entities.create = function(entityClass, entityName)
    if entities[entityClass] ~= nil then
        return entities[entityClass](entityName)
    else
        return createEntity(entityClass, entityName)
    end
end