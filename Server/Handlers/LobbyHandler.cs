using System;
using System.Collections.Generic;
using System.Linq;
using Data.Model;
using GameServer.Game;
using GameServer.New;
using GameServer.Util;
using Network;
using Network.Packets.Client;
using Network.Packets.Client.Inventory;
using Network.Packets.Client.Lobby;
using Network.Packets.Server.Lobby;
using Swan.Logging;
using GameState = Game.GameState;
using GameStatus = Data.GameStatus;

namespace GameServer.Handlers
{
    /// <summary>
    /// This class handles rooms, user switches, etc
    /// </summary>
    public static class LobbyHandler
    {
        /// <summary>
        /// All current rooms
        /// </summary>
        private static readonly SortedList<int, NewRoom> Rooms = new SortedList<int, NewRoom>();

        /// <summary>
        /// Try to create a room
        /// </summary>
        /// <returns></returns>
        [PacketHandler(ClientPacketType.CreateGame)]
        public static void OnCreateGame(GameClient client, CreateGamePacket packet)
        {
            // Hold our result packet here
            var result = new GameCreatedPacket
            {
                ResultCode = GameCreatedResultCode.Failed,
                Master = client.User,
            };
            
            // Create room
            var room = new NewRoom();
            
            // Set record
            room.SetRoomRecord(packet.RoomRecord);
            
            // Set master
            room.SetMaster(client.UserId);
            
            // Add to rooms dictionary
            var id = Rooms.AddNext(room);

            // Assign generated id
            room.RoomRecord.Id = id;
            
            // TODO: Check for anything that might cause an error
            result.ResultCode = GameCreatedResultCode.Success;
            
            // Assign room to packet
            result.RoomRecord = room.RoomRecord;
            result.GameStats = room.GameStats;
            
            // Send to them
            client.Send(PacketFactory.CreatePacket(result));
            
            // Log
            $"Created room {room}. Result is {result.ResultCode}".Debug(client.ToString());
        }

        /// <summary>
        /// Sent when the client would like to enter a game
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        [PacketHandler(ClientPacketType.EnterGame)]
        public static void OnTryEnterGame(GameClient client, EnterGamePacket packet)
        {
            // Hold our result packet here
            var result = new GameEnteredPacket
            {
                ResultCode = GameEnteredResult.NotAvailable,
            };
            
            // Get room
            var room = GetRoomById(packet.RoomId);
            
            // If room doesnt exist
            if (room != null)
            {
                // If game is full
                if (room.Clients.Count() >= room.Capacity)
                {
                    result.ResultCode = GameEnteredResult.Full;
                }
                else
                {
                    // TODO: Other checks, too early, banned, no units, etc
                }
            
                // Success condition
                result.ResultCode = GameEnteredResult.Success;

                // Fill in packet
                result.Master = room.Master;
                result.GameStats = room.GameStats;
                result.RoomRecord = room.RoomRecord;
                result.PlayerCount = room.Clients.Count();
                result.GameStatus = room.GameStatus == GameState.WaitingRoom ? GameStatus.Waiting : GameStatus.InPlay;
                
                // Call into room to join player
                room.AddPlayer(client);
            }
            
            // Send to them
            client.Send(PacketFactory.CreatePacket(result));

            // Log
            $"Tried to join room {room}. Result is {result.ResultCode}".Debug(client.ToString());
        }

        /// <summary>
        /// Deletes a room by its id
        /// TODO: Kick users back to menu?
        /// TODO: Error handling
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteRoom(int id)
        {
            // Remove
            Rooms.Remove(id, out var room);
            
            // Dispose
            room?.Dispose();
        }

        /// <summary>
        /// Finds a room by its id
        /// TODO: Error handling
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static NewRoom GetRoomById(int id)
        {
            return Rooms[id];
        }

        /// <summary>
        /// Gets all the rooms
        /// TODO: Filters?
        /// </summary>
        /// <returns></returns>
        public static List<NewRoom> GetRooms()
        {
            return Rooms.Values.ToList();
        }
    }
    
    
}