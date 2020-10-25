using System;
using System.Collections.Generic;
using System.Linq;
using Data.Model;
using GameServer.Game;
using GameServer.Util;
using Swan.Logging;

namespace GameServer.Managers
{
    /// <summary>
    /// This class handles rooms, user switches, etc
    /// TODO: See what needs to go into database and what can stay in memory
    /// </summary>
    public static class RoomManager
    {
        /// <summary>
        /// All current rooms
        /// </summary>
        private static readonly SortedList<int, RoomInstance> Rooms = new SortedList<int, RoomInstance>();

        /// <summary>
        /// Creates a new room and returns its ID
        /// </summary>
        /// <returns></returns>
        public static bool CreateRoom(RoomInstance room)
        {
            try
            {
                room.SettingsChanged();
            }
            catch (Exception e)
            {
                $"Unable to create room with tempate {room.GameTemplate} - {e.Message}!".Warn();
                return false;
            }
            
            var id = Rooms.AddNext(room);

            room.Id = id;

            return true;
        }

        /// <summary>
        /// Deletes a room by its id
        /// TODO: Kick users back to menu?
        /// TODO: Error handling
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteRoom(int id)
        {
            Rooms.Remove(id);
        }

        /// <summary>
        /// Finds a room by its id
        /// TODO: Error handling
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static RoomInstance GetRoomById(int id)
        {
            return Rooms[id];
        }

        /// <summary>
        /// Gets all the rooms
        /// TODO: Filters?
        /// </summary>
        /// <returns></returns>
        public static List<RoomInstance> GetRooms()
        {
            return Rooms.Values.ToList();
        }
    }
    
    
}