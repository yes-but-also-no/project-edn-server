using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Data.Model;
using GameServer.ServerPackets;
using GameServer.ServerPackets.Chat;
using GameServer.Util;
using Swan.Logging;
using Console = Colorful.Console;

namespace GameServer.Managers
{
    /// <summary>
    /// This manager controls all chat channels in the game
    /// </summary>
    public static class ChatManager
    {
        /// <summary>
        /// All current rooms
        /// </summary>
        public static readonly SortedList<int, ChatChannel> Channels = new SortedList<int, ChatChannel>();
        
        /// <summary>
        /// Creates a new chat channel and returns its ID
        /// </summary>
        /// <returns></returns>
        public static int CreateChannel(string name)
        {
            var chan = new ChatChannel {Name = name};
            
            var id = Channels.AddNext(chan);

            chan.Id = id;

            return id;
        }
        
        /// <summary>
        /// Creates a new chat channel and returns its ID
        /// </summary>
        /// <returns></returns>
        public static int CreateChannel(ChatChannel chan)
        {           
            var id = Channels.AddNext(chan);

            chan.Id = id;

            return id;
        }
        
        /// <summary>
        /// Deletes a channel by its id
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteChannel(int id)
        {
            Channels.Remove(id);
        }

        /// <summary>
        /// Joins a channel by name
        /// </summary>
        /// <param name="user"></param>
        /// <param name="channel"></param>
        public static MsgJoinChannelResult TryJoinChannel(GameSession session, string channel)
        {
            // Find channel
            var chan = Channels.Values.FirstOrDefault(c => c.Name == channel);

//            foreach (var chatChannel in Channels.Values)
//            {
//                Console.WriteLine($"Channel: id - {chatChannel.Id} name - {chatChannel.Name}");
//            }

            // If its not there for some reason, error it
            if (chan == null) return new MsgJoinChannelResult(false);
            
            // Join it
            chan.JoinChannel(session);
            
            // Return success
            return new MsgJoinChannelResult(true, chan);
        }

        /// <summary>
        /// Broadcasts a message everywhere
        /// </summary>
        /// <param name="message"></param>
        public static void BroadcastAll(string message)
        {
            foreach (var chan in Channels.Values)
            {
                chan.SendMessage(message);
            }
        }

    }

    /// <summary>
    /// A chat channel is a single chat room instance
    /// </summary>
    public class ChatChannel
    {
        /// <summary>
        /// Id of this channel
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The name of this channel
        /// </summary>
        public string Name { get; set; }
        
        public Action<GameSession, ChatChannel> OnConnect { get; set; }
        
        /// <summary>
        /// Dictionary of all sessions in this channel
        /// </summary>
        private readonly ConcurrentDictionary<Guid, GameSession> _sessions = new ConcurrentDictionary<Guid, GameSession>();
        
        /// <summary>
        /// The users in this channel
        /// </summary>
        public List<ExteelUser> Users => _sessions.Values.Select(s => s.User).ToList();
        
        /// <summary>
        /// Multicasts a packet to all users in this room
        /// </summary>
        /// <param name="packet"></param>
        private void MulticastPacket(ServerBasePacket packet)
        {
            var data = packet.Write();
            
            // Multicast data to all sessions
            foreach (var session in _sessions.Values)
                session.SendAsync(data);
        }

        /// <summary>
        /// Adds a session to this channel
        /// </summary>
        /// <param name="session"></param>
        public void JoinChannel(GameSession session)
        {
            // Set session
            session.Channel = this;
            
            // Broadcast join to channel
            MulticastPacket(new MsgUserJoinedChannel(session.User, Id));
            
            // Add user to channel
            _sessions.TryAdd(session.Id, session);
            
            // Log
            $"[{session}] has joined".Info(source: ToString());
            
            // Trigger hook if its there
            OnConnect?.Invoke(session, this);
        }
        
        /// <summary>
        /// Removes a session from this room
        /// </summary>
        /// <param name="id"></param>
        private void RemoveSession(Guid id)
        {
            // Unregister session by Id
            _sessions.TryRemove(id, out GameSession temp);
        }
        
        /// <summary>
        /// Removes a user from the channel
        /// </summary>
        /// <param name="session"></param>
        public void LeaveChannel(GameSession session)
        {           
            // Remove from session list
            RemoveSession(session.User.SessionId);
            
            // Clear data
            session.Channel = null;
            
            // Broadcast leave data here
            MulticastPacket(new MsgUserLeftChannel(session.UserId, Id));

            // Log
            $"[{session}] has left".Info(source: ToString());
        }

        /// <summary>
        /// Sends a message to the channel
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        public void SendMessage(GameSession session, string message)
        {
            MulticastPacket(new MsgChannelChatting(session.UserId, Id, message));
            
            // Log
            $"[{session}]: {message}".Info(source: ToString());
        }
        
        /// <summary>
        /// Sends a message to the channel
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            MulticastPacket(new MsgChannelChatting(0, Id, message));
            
            // Log
            $"[BROADCAST]: {message}".Info(source: ToString());
        }
        
        /// <summary>
        /// Sends a message to a specific user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        public void SendMessageToSession(GameSession session, string message)
        {
            session.SendPacket(new MsgChannelChatting(0, Id, message));
        }
        
        public override string ToString()
        {
            return $"[CHAT_CHANNEL]<{Id}:{Name}>";
        }
    }
}