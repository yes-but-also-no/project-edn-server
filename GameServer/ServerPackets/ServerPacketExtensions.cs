using System.Drawing;
using System.Linq;
using Colorful;
using Data.Model;
using Data.Model.Items;
using GameServer.Configuration.Poo;
using GameServer.Game;
using GameServer.Model.Parts;
using GameServer.Model.Parts.Weapons;

namespace GameServer.ServerPackets
{
    /// <summary>
    /// Extensions for writing common data to packets
    /// </summary>
    public static class ServerPacketExtensions
    {
        /// <summary>
        /// Writes pilot info into a packet
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="info"></param>
        public static void WritePilotInfo(this ServerBasePacket packet, PilotInfo info)
        {
            packet.WriteInt(info.AbilityPointsAvailable);
            
            packet.WriteByte((byte)info.HpLevel);
            packet.WriteByte((byte)info.MoveSpeedLevel);
            packet.WriteByte((byte)info.EnLevel);
            packet.WriteByte((byte)info.ScanRangeLevel);
            packet.WriteByte((byte)info.SpLevel);
            packet.WriteByte((byte)info.AimLevel);
            
            packet.WriteByte(0); // Unknown
            packet.WriteByte(0); // Unknown
            
            packet.WriteInt(info.MeleeLevel);
            packet.WriteInt(info.RangedLevel);
            packet.WriteInt(info.SiegeLevel);
            packet.WriteInt(info.RocketLevel);
            
            //packet.WriteInt(0); // Unknown
            packet.WriteByte(0); // NOTE: Not sure why everything after here is offset by one, so i am just doing 3 bytes instead of 4 here
            packet.WriteByte(0);
            packet.WriteByte(0);
        }

        /// <summary>
        /// Writes a parts info into a packet
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="part"></param>
        public static void WritePartInfo(this ServerBasePacket packet, PartBase part)
        {
            if (!(part is NullWeapon))
            {
                packet.WriteInt(part.Id);
                packet.WriteUInt(part.TemplateId);

                packet.WriteUShort(0x86a0); // Parameter - Possibly for permanent / cannot delete items?
                packet.WriteUShort(0); // Type? - Zero in packets

                packet.WriteByte(part.Color.R);
                packet.WriteByte(part.Color.G);
                packet.WriteByte(part.Color.B);

                // 0 - Head
                // 1 - Chest
                // 2 - Arm
                // 3 - Leg
                // 4 - Backpack
                // 5 - Fist
                // 6 - Gun
                // 7 - Shield
                
                // Quick and dirty, get first digit
                var partsType = part.TemplateId;
                while (partsType >= 10)
                    partsType /= 10;

                partsType--;
                
                packet.WriteByte((byte)partsType); // Looks like 0 indexed version of part type 
                packet.WriteByte(0); // Unknown

                packet.WriteInt(0); // Unknown

                // FArray - Unknown
                packet.WriteInt(0); // Array size

                // Array contents would go here

                packet.WriteInt(-1); // Expiry time / current durability
                packet.WriteInt(0x14997000); // Max Durability - Value from server packet capture
                packet.WriteInt(2); // If 1, durability, if 2, expired?
                packet.WriteInt(0); // Unknown
            }
            else
            {
                packet.WriteInt(-1);
                packet.WriteUInt(0);

                packet.WriteUShort(0);
                packet.WriteUShort(0);

                packet.WriteByte(0);
                packet.WriteByte(0);
                packet.WriteByte(0);

                packet.WriteByte(0); // Unknown
                packet.WriteByte(0); // Unknown

                packet.WriteInt(0); // Unknown

                // FArray - Unknown
                packet.WriteInt(0); // Array size

                // Array contents would go here

                packet.WriteInt(0); // Unknown
                packet.WriteInt(0); // Unknown
                packet.WriteInt(0); // If 1, durability, if 2, expired?
                packet.WriteInt(0); // Unknown
            }
        }
        
         /// <summary>
        /// Writes a parts info into a packet
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="part"></param>
        public static void WritePartInfo(this ServerBasePacket packet, PartRecord part)
        {
            if (part != null)
            {
                packet.WriteInt(part.Id);
                packet.WriteUInt(part.TemplateId);

                packet.WriteUShort(0x86a0); // Parameter - Possibly for permanent / cannot delete items?
                packet.WriteUShort(0); // Type? - Zero in packets

                packet.WriteByte(part.Color.R);
                packet.WriteByte(part.Color.G);
                packet.WriteByte(part.Color.B);

                // 0 - Head
                // 1 - Chest
                // 2 - Arm
                // 3 - Leg
                // 4 - Backpack
                // 5 - Fist
                // 6 - Gun
                // 7 - Shield
                
                // Quick and dirty, get first digit
                var partsType = part.TemplateId;
                while (partsType >= 10)
                    partsType /= 10;

                partsType--;
                
                packet.WriteByte((byte)partsType); // Looks like 0 indexed version of part type 
                packet.WriteByte(0); // Unknown

                packet.WriteInt(0); // Unknown

                // FArray - Unknown
                packet.WriteInt(0); // Array size

                // Array contents would go here

                packet.WriteInt(-1); // Expiry time / current durability
                packet.WriteInt(0x14997000); // Max Durability - Value from server packet capture
                packet.WriteInt(2); // If 1, durability, if 2, expired?
                packet.WriteInt(0); // Unknown
            }
            else
            {
                packet.WriteInt(-1);
                packet.WriteUInt(0);

                packet.WriteUShort(0);
                packet.WriteUShort(0);

                packet.WriteByte(0);
                packet.WriteByte(0);
                packet.WriteByte(0);

                packet.WriteByte(0); // Unknown
                packet.WriteByte(0); // Unknown

                packet.WriteInt(0); // Unknown

                // FArray - Unknown
                packet.WriteInt(0); // Array size

                // Array contents would go here

                packet.WriteInt(0); // Unknown
                packet.WriteInt(0); // Unknown
                packet.WriteInt(0); // If 1, durability, if 2, expired?
                packet.WriteInt(0); // Unknown
            }
        }

        /// <summary>
        /// Writes room info onto a packet
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="room"></param>
        public static void WriteRoomInfo(this ServerBasePacket packet, RoomInstance room)
        {
            // TODO: Move this to helper or maybe even room info obj
            packet.WriteString("isishqduarte.mynetgear.com"); // Room server... If this does not match current server, it does a switch
            
            packet.WriteInt(room.Id); // Room Id
            packet.WriteInt(room.MasterId); // Unknown - maybe master?
            packet.WriteInt((int)room.GameType); // Game type

            
            packet.WriteString(room.Name);
            packet.WriteString(room.Password); // Unknown - Empty in packet
            packet.WriteString("Face Off"); // Map name?
            packet.WriteString(room.Master != null ? room.Master.Callsign : "UNKNOWN");
  
            packet.WriteInt(room.Capacity);
            packet.WriteInt(room.Users.Count());
  
            packet.WriteBool(!string.IsNullOrEmpty(room.Password)); // Game is private?
            packet.WriteByte(1); // Unknown

            packet.WriteInt((int)room.GameStatus); // Status?
            packet.WriteUInt(room.GameTemplate); // Not sure why this is sent twice?
            packet.WriteUInt(room.GameTemplate); // This was supposed to be adhoc id

            packet.WriteBool(room.SdMode); // SD Battle

            packet.WriteInt(0); // Unknown - zero on packet
            packet.WriteInt(0x000927c0); // Unknown - zero on packet
            packet.WriteInt(0x000668a0); // Unknown
            packet.WriteInt(0x000927c0); // Unknown
            packet.WriteInt(0); // Unknown
  
            packet.WriteBool(room.Balance); // Unknown

            packet.WriteInt(room.GameType == GameType.defensivebattle ? room.Difficulty : -1); // Unknown
 
            packet.WriteBool(room.FiveMinute); // Unknown
            
            packet.WriteInt(room.MaxLevel); // max level
            packet.WriteInt(room.MinLevel); // min Level
            packet.WriteInt(0); // Unknown
            packet.WriteInt(0); // Unknown
            packet.WriteInt(0); // Unknown
        }

        public static void WriteRoomUserInfo(this ServerBasePacket packet, RoomInstance room, ExteelUser user)
        {
            packet.WriteString(user.Callsign); // Unknown - Looks like 2x callsign from packet cap
            packet.WriteString(user.Callsign);
            packet.WriteString(""); // Clan name
            
            packet.WriteInt(0); // Unknown
            packet.WriteInt(0); // Unknown - clan id?
            
            packet.WriteInt(user.Id);
            packet.WriteUInt(user.Team);
            
            packet.WriteUInt(user.Rank); // Unknown
            packet.WriteInt(0); // Unknown
            packet.WriteInt(0); // Unknown
            packet.WriteInt(0); // Unknown
            packet.WriteInt(0); // Unknown
            packet.WriteInt(0); // Unknown
            
            
            packet.WriteBool(user.IsReady); // Unknown - ready?
            packet.WriteBool(user.Id == room.MasterId);
            
            for (var i = 0; i < 13; i++)
            {
                packet.WriteInt(0); //Unknown
            }
            
            for (var i = 0; i < 12; i++)
            {
                packet.WriteInt(0); //Unknown
            }
            
            for (var i = 0; i < 13; i++)
            {
                packet.WriteInt(0); //Unknown
            }
            
            for (var i = 0; i < 14; i++)
            {
                packet.WriteInt(0); // Unknown
            }
            
            for (var i = 0; i < 11; i++)
            {
                packet.WriteInt(0); // Unknown
            }
            
            for (var i = 0; i < 11; i++)
            {
                packet.WriteInt(0); // Unknown
            }
            
            // Unknown struct
            packet.WriteInt(0); // 
            packet.WriteInt(0); // 
            packet.WriteInt(0); // 
            packet.WriteInt(0); // 
            packet.WriteInt(0); // 
            packet.WriteInt(0); // 
            packet.WriteInt(0); // 
            packet.WriteInt(0); // 
            
            // Another unknown struct. Seems to be an array?
            packet.WriteInt(0); // Size?
            
            
            
            packet.WritePilotInfo(user.PilotInfo);
        }
    }
}