using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sylver.Network.Data;

namespace Network
{
    /// <summary>
    /// This class manages all packet handlers
    /// </summary>
    public static class PacketHandler
    {
        /// <summary>
        /// Dictionary of all handlers
        /// </summary>
        private static readonly Dictionary<object, Action<GameClient, INetPacketStream>> Handlers 
            = new Dictionary<object, Action<GameClient, INetPacketStream>>();
        
        /// <summary>
        /// Packet handler method struct
        /// </summary>
        private struct PacketMethodHandler : IEquatable<PacketMethodHandler>
        {
            public readonly PacketHandlerAttribute Attribute;
            public readonly MethodInfo Method;

            public PacketMethodHandler(MethodInfo method, PacketHandlerAttribute attribute)
            {
                Method = method;
                Attribute = attribute;
            }

            public bool Equals(PacketMethodHandler other)
            {
                return Attribute.Header == other.Attribute.Header
                       && Attribute.TypeId == other.Attribute.TypeId
                       && Method == other.Method;
            }
        }
        
        public static void RegisterAssembly(Assembly assembly)
        {
            IEnumerable<PacketMethodHandler[]> handlers = from type in assembly.GetTypes()
                                                          let typeInfo = type.GetTypeInfo()
                                                          let methodsInfo = typeInfo.GetMethods(BindingFlags.Static | BindingFlags.Public)
                                                          let handler = (from x in methodsInfo
                                                                         let attribute = x.GetCustomAttribute<PacketHandlerAttribute>()
                                                                         where attribute != null
                                                                         select new PacketMethodHandler(x, attribute)).ToArray()
                                                          select handler;

            foreach (PacketMethodHandler[] handler in handlers)
            {
                foreach (PacketMethodHandler methodHandler in handler)
                {
                    ParameterInfo[] parameters = methodHandler.Method.GetParameters();

                    if (parameters.Count() < 2 || parameters.First().ParameterType != typeof(GameClient))
                        continue;

                    var action = methodHandler.Method.CreateDelegate(typeof(Action<GameClient, INetPacketStream>)) as Action<GameClient, INetPacketStream>;

                    Handlers.Add(methodHandler.Attribute.Header, action);
                }
            }
        }

        public static bool Invoke(GameClient invoker, INetPacketStream packet, object packetHeader)
        {
            if (!Handlers.TryGetValue(packetHeader, out Action<GameClient, INetPacketStream> packetHandler))
                return false;

            try
            {
                packetHandler.Invoke(invoker, packet);
            }
            catch (Exception e)
            {
                throw new Exception($"An error occured during the execution of packet handler: {packetHeader}", e);
            }

            return true;
        }
    }
}