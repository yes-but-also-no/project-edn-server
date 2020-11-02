using Engine.Entities;
using MoonSharp.Interpreter;

namespace Game
{
    /// <summary>
    /// Test proxy wrapper for lua access
    /// </summary>
    public class EntityProxy
    {
        private Entity _target;

        [MoonSharpHidden]
        public EntityProxy(Entity t)
        {
            _target = t;
        }

        public string EngineName => _target.EngineName;
    }
}