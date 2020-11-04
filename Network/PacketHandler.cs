using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Network.Packets;
using Network.Packets.Client;
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
        private static readonly Dictionary<ClientPacketType, PacketMethodHandler> Handlers 
            = new Dictionary<ClientPacketType, PacketMethodHandler>();
        
        /// <summary>
        /// Packet handler method struct
        /// </summary>
        private struct PacketMethodHandler : IEquatable<PacketMethodHandler>
        {
            public readonly PacketHandlerAttribute Attribute;
            public readonly MethodInfo Method;
            public readonly Type PacketType;

            public PacketMethodHandler(MethodInfo method, PacketHandlerAttribute attribute)
            {
                Method = method;
                Attribute = attribute;
                PacketType = Method.GetParameters()[1].ParameterType;
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
                    
                    Handlers.Add(methodHandler.Attribute.Header, methodHandler);
                }
            }
        }

        /// <summary>
        /// This will match the packet id against a packet deserializer, deserialize it, and call the appropriate handler
        /// </summary>
        /// <param name="invoker"></param>
        /// <param name="packet"></param>
        /// <param name="packetHeader"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool Invoke(GameClient invoker, INetPacketStream packet, ClientPacketType packetHeader)
        {
            if (!Handlers.TryGetValue(packetHeader, out PacketMethodHandler packetHandler))
                throw new HandlerNotFoundException();

            try
            {
                var deserializer = (IPacketDeserializer)Activator.CreateInstance(packetHandler.PacketType);
                
                deserializer?.Deserialize(packet);
                
                packetHandler.Method.Invoke(null, new object[]{invoker, deserializer});
            }
            catch (Exception e)
            {
                throw new Exception($"An error occured during the execution of packet handler: {packetHeader}", e);
            }

            return true;
        }

        /// <summary>
        /// Thrown if there is no registered handler for a packet type
        /// </summary>
        public class HandlerNotFoundException : Exception
        {
        }
    }
}