using System;
using System.Linq;
using System.Net.Sockets;
using Data;
using Data.Configuration;
using Data.Model;
using Microsoft.EntityFrameworkCore;
using Network;
using Network.Packets.Client;
using Swan.Logging;
using Sylver.Network.Data;
using Sylver.Network.Server;

namespace GameServer.New
{
    /// <summary>
    /// This represents a single connect net client
    /// </summary>
    public class GameClient : NetServerClient
    {
        public GameClient(Socket socketConnection) : base(socketConnection)
        {
        }

        /// <summary>
        /// Called when a packet is recieved from the client
        /// </summary>
        /// <param name="packet"></param>
        public override void HandleMessage(INetPacketStream packet)
        {

            // Temporary variables
            byte packetId = 0;
            ClientPacketType packetType;

            try
            {
                // Read packed id
                packetId = packet.Read<byte>();

                // Attempt to cast to packet id
                packetType = (ClientPacketType) packetId;

                // Invoke handler
                PacketHandler<GameClient>.Invoke(this, packet, packetType);

                // Log
                if (ServerConfig.Configuration.Global.LogAllPackets)
                    $"[C] 0x{packetId:x2} {Enum.GetName(typeof(ClientPacketType), packetId)}".Info(ToString());
            }
            catch (PacketHandler<GameClient>.HandlerNotFoundException)
            {
                // Check for missing handler
                if (Enum.IsDefined(typeof(ClientPacketType), packetId))
                {
                    $"Received an unimplemented packet {Enum.GetName(typeof(ClientPacketType), packetId)} ({packetId:x2}) from {Socket.RemoteEndPoint}."
                        .Warn(ToString());
                }
                else
                {
                    $"Received an unknown packet {packetId:x2} from {Socket.RemoteEndPoint}.".Warn(ToString());
                }
            }
            catch (Exception exception)
            {
                // All other errors
                $"Error occured in handle message from {Socket.RemoteEndPoint}: {exception.InnerException}".Error(ToString());
            }
        }
        
        #region USER
        
        /// <summary>
        /// Current user id
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// User object reference
        /// </summary>
        public ExteelUser User { get; private set; }
        
        /// <summary>
        /// Gets the callsign for this session if they are logged in
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            return User != null ? User.Callsign : "[NOT SIGNED IN]";
        }
        
        /// <summary>
        /// Updates user with data from database
        /// </summary>
        public void UpdateUserFromDatabase()
        {
            if (UserId == 0) return;
            
            using (var db = new ExteelContext())
            {               
                // Find user
                var user = db.Users.SingleOrDefault(u => u.Id == UserId);
                
                // TODO: Check for ban state / admin state / user already logged in
                
                // Successful login from here, so assign user
                
                // Load user inventory
                user.Inventory = db.Inventories
                    .Include(i => i.Parts)
                    
                    .Include(i => i.Units)
                        .ThenInclude(u => u.Head)
                    
                    .Include(i => i.Units)
                        .ThenInclude(u => u.Chest)
                    
                    .Include(i => i.Units)
                        .ThenInclude(u => u.Arms)
                    
                    .Include(i => i.Units)
                        .ThenInclude(u => u.Legs)
                    
                    .Include(i => i.Units)
                        .ThenInclude(u => u.Backpack)
                    
                    .Include(i => i.Units)
                        .ThenInclude(u => u.WeaponSet1Left)
                    
                    .Include(i => i.Units)
                        .ThenInclude(u => u.WeaponSet1Right)
                    
                    .Include(i => i.Units)
                        .ThenInclude(u => u.WeaponSet2Left)
                    
                    .Include(i => i.Units)
                        .ThenInclude(u => u.WeaponSet2Right)
                    
                    .SingleOrDefault(i => i.Id == user.InventoryId);


                // Load users stats
                db
                    .Entry(user)
                    .Collection(u => u.Stats)
                    .Load();
                
                // Assign to client
                User = user;
            }
        }
        
        #endregion
        
        #region PLAYER
        
        /// <summary>
        /// This users player id. Only used in room and game
        /// </summary>
        public Guid PlayerId { get; set; }
        
        #endregion
        
        public override string ToString()
        {
            return $"[GameClient]<{Id}><{GetUserName()}>";
        }
    }
}