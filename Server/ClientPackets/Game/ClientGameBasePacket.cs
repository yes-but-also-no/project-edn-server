using System;
using System.Numerics;
using Data.Model;
using GameServer.Model.Units;
using GameServer.ServerPackets.Game;
using Console = Colorful.Console;

namespace GameServer.ClientPackets.Game
{
    /// <summary>
    /// Base packet for game packets from the client
    /// </summary>
    public abstract class ClientGameBasePacket : ClientBasePacket
    {
        /// <summary>
        /// The unit for this client
        /// </summary>
        protected readonly Unit Unit;
        
        protected ClientGameBasePacket(byte[] data, GameSession client) 
            : base(data, client)
        {
            Unit = GetClient().CurrentUnit;
        }

        /// <summary>
        /// Ticks the unit by reading the current time stamp
        /// TODO: Should this be called during the send step?
        /// </summary>
        protected void TickUnit()
        {
            // Just for practice mode right now
            if (Unit == null) return;
            
            var ping = GetUInt();
            
            // TODO: I think this ping is just for checking for desync, dont need to use
            
            //var delta = Unit.UpdatePing(ping);
            
            //GetClient().GameInstance?.TickUnit(Unit, delta);
        }

        /// <summary>
        /// Reads the units current aim and pos values from the packet
        /// Usually located at the end of the packet
        /// </summary>
        protected void GetUnitPositionAndAim()
        {
            // TODO: Make this better
            // Just for practice mode right now
            if (Unit == null) return;
            
            // Read aim
            Unit.AimY = GetUShort();
            Unit.AimX = GetUShort();

//            Console.WriteLine("User Aim X: {1} Aim Y: {0}", y, x);

            // Save old position
            Unit.LastWorldPosition = Unit.WorldPosition;
            
            // Update new position
            Unit.WorldPosition = new Vector3(GetFloat(), GetFloat(), GetFloat());
            
            // If they in jumped they in air
            if (Unit.Boosting == 2)
            {
                Unit.InAir = true;
            }
            else
            {
                // If they have stayed still, no longer in air 
                // TODO: DONT FUCKING GO IN THE GROUND JESUS
                if (Unit.WorldPosition.Z == Unit.LastWorldPosition.Z && Unit.InAir)
                {
                    Unit.InAir = false;
                }
            }
        }
    }
}