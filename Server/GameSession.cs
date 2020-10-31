using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Data;
using Data.Model;
using GameServer.ClientPackets;
using GameServer.Game;
using GameServer.Managers;
using GameServer.Model.Units;
using GameServer.ServerPackets;
using Microsoft.EntityFrameworkCore;
using NetCoreServer;
using Swan.Logging;
using Console = Colorful.Console;

namespace GameServer
{
    /// <summary>
    /// The game session represents a single connected user
    /// The user may or may not be logged in
    /// </summary>
    public class GameSession : TcpSession
    {
        /// <summary>
        /// The user logged into this session
        /// </summary>
        public ExteelUser User { get; private set; }
        
        /// <summary>
        /// The user id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The protocol version of this client
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// The time stamp of when the client connected
        /// </summary>
        public DateTime ConnectedStamp { get; set; }

        public int ConnectedMs => (int)(DateTime.UtcNow - ConnectedStamp).TotalMilliseconds;
        
        /// <summary>
        /// The room this session is connected to
        /// </summary>
        public RoomInstance RoomInstance { get; set; }

        /// <summary>
        /// Quick accessor for game instance
        /// </summary>
        public GameInstance GameInstance => RoomInstance?.GameInstance;
        
        /// <summary>
        /// The current unit this session is controlling
        /// Only used in game
        /// </summary>
        public Unit CurrentUnit { get; set; }
        
        /// <summary>
        /// The channel this session is in
        /// </summary>
        public ChatChannel Channel { get; set; }
        
        /// <summary>
        /// Has this session loaded into the game fully?
        /// TODO: This should probably be somewhere else
        /// TODO: Reset this to false after game
        /// </summary>
        public bool IsGameReady { get; set; }

        /// <summary>
        /// Is the user signed in?
        /// </summary>
        public bool IsSignedIn => User != null;

        public GameSession(TcpServer server) : base(server)
        {
            
        }

        /// <summary>
        /// Gets the username for this session if they are logged in
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            return User != null ? User.Username : "[NOT SIGNED IN]";
        }

        public override string ToString()
        {
            return $"[SESSION]<{Id}:{GetUserName()}>";
        }

        /// <summary>
        /// Send a packet to this client
        /// </summary>
        /// <param name="packet"></param>
        public void SendPacket(ServerBasePacket packet)
        {
            var data = packet.Write();

            if (packet.HighPriority)
                Send(data);
            else
                SendAsync(data);
            
            if (Program.Configuration.Global.LogAllPackets)
                $"[S] 0x{packet.GetId():x2} {packet.GetType()} >>> {GetUserName()}".Info();
            
            // TODO: Add config here - if debug
            // TODO: Temp disabled to mute spam
           // Console.WriteLine("[S] 0x{0:x2} {1} >>> {2}", Color.Green, packet.GetId(), packet.GetType(), GetUserName());
        }

        protected override void OnConnected()
        {
            // Stamp when they connect for calculating ping
            ConnectedStamp = DateTime.UtcNow;
            
            var remoteIpEndPoint = Socket.RemoteEndPoint as IPEndPoint;
            var localIpEndPoint = Socket.LocalEndPoint as IPEndPoint;
            
            $"[{ConnectedStamp}][{remoteIpEndPoint?.Address}][{localIpEndPoint?.Port}] Game TCP session with Id {Id} connected!".Info();
        }

        protected override void OnDisconnected()
        {
            $"Game TCP session with Id {Id} disconnected!".Info();
            
            // If they are in game, broadcast quit
            RoomInstance?.TryLeaveGame(this);
            Channel?.LeaveChannel(this);
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            //Console.WriteLine("GOT PACKET: " + DateTime.UtcNow.Millisecond, Color.Blue);
            var pos = 0;
            while (pos < size)
            {
                var packet = PacketHandler.HandlePacket(buffer, pos, this);
                pos += packet.Size;
                
                // TODO: Add config here - if debug
                // Temp - only unknowns
                if (Program.Configuration.Global.LogAllPackets && !(packet is UnknownPacket))
                    $"[C] 0x{packet.Id:x2} {packet.GetType()} <<< {GetUserName()}".Info();
                
                if (packet is UnknownPacket)
                    $"[C] 0x{packet.Id:x2} {packet.GetType()} <<< {GetUserName()}".Warn();

                // TODO: RUN PACKET ASYNC IF LOW PRIORITY?
                packet.Run();
            }
        }

        protected override void OnError(SocketError error)
        {
            $"Game TCP session caught an error with code {error}".Warn();
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
                    
                    // .Include(i => i.Units)
                    //     .ThenInclude(u => u.Skill1)
                    //
                    // .Include(i => i.Units)
                    //     .ThenInclude(u => u.Skill2)
                    //
                    // .Include(i => i.Units)
                    //     .ThenInclude(u => u.Skill3)
                    //
                    // .Include(i => i.Units)
                    //     .ThenInclude(u => u.Skill4)
                    //
                    .SingleOrDefault(i => i.Id == user.InventoryId);
                
                // Assign user units
                // TODO: Maybe we can map this, not sure
                //user.Inventory.Units.ForEach(u => u.User = user);
                
                
                // Load users stats
                db
                    .Entry(user)
                    .Collection(u => u.Stats)
                    .Load();
                
                // Assign to client
                User = user;
                
                // Set session
                User.SessionId = Id;
            }
        }
    }
}