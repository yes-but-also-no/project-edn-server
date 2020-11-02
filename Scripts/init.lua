print("Hello world!")

local ent = createEntity("TestEntity", "lua_created_entity")

print(getmetatable(ent))

local test = {
    EngineName = "wtf"
}

function ent:TestFunction()
    print(self.EngineName)
end

print('test')

test:TestFunction()