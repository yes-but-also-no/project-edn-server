using System.IO;
using System.Numerics;
using System.Text;
using Data.Model;
using Data.Model.Items;
using Network.Packets.Server;
using Sylver.Network.Data;

namespace Network
{
    /// <summary>
    /// Represents a single game packet
    /// </summary>
    public class GamePacket : NetPacketStream
    {
        public const byte NullId = 0xFF;
        
        /// <inheritdoc />
        protected override Encoding ReadEncoding => Encoding.Unicode;

        /// <inheritdoc />
        protected override Encoding WriteEncoding => Encoding.Unicode;
        
        /// <summary>
        /// Gets the GamePacket buffer.
        /// </summary>
        public override byte[] Buffer => BuildPacketBuffer();
        
        /// <summary>
        /// Creates a new GamePacket in write-only mode.
        /// </summary>
        public GamePacket()
        {
            base.Write<ushort>(0);
        }

        /// <summary>
        /// Creates a new GamePacket with a header.
        /// </summary>
        /// <param name="packetHeader"></param>
        public GamePacket(ServerPacketType packetHeader)
            : this()
        {
            WriteHeader(packetHeader);
        }

        /// <summary>
        /// Creates a new GamePacket in read-only mode.
        /// </summary>
        /// <param name="buffer"></param>
        public GamePacket(byte[] buffer)
            : base(buffer)
        {
        }

        /// <summary>
        /// Write packet header.
        /// </summary>
        /// <param name="packetHeader">GamePacket header</param>
        public void WriteHeader(ServerPacketType packetHeader) => Write((byte)packetHeader);
        
        /// <summary>
        /// Builds the packet buffer.
        /// </summary>
        /// <returns></returns>
        private byte[] BuildPacketBuffer()
        {
            long oldPointer = Position;

            Seek(0, SeekOrigin.Begin);
            base.Write((ushort)Length);
            Seek(oldPointer, SeekOrigin.Begin);

            return base.Buffer;
        }
        
        /// <summary>
        /// Reads a vector
        /// </summary>
        /// <returns></returns>
        public Vector3 ReadVector()
        {
            return new Vector3(
                    Read<float>(),
                    Read<float>(),
                    Read<float>()
                );
        }

        /// <summary>
        /// Special override for strings, which use 2 byte lengths instead of 4
        /// and are null terminated
        /// </summary>
        /// <returns></returns>
        public string ReadGameString()
        {
            var stringLength = Read<short>();
            var stringBytes = Read<byte>(stringLength);

            return ReadEncoding.GetString(stringBytes);
        }
        
        /// <summary>
        /// Writes a vector
        /// </summary>
        /// <returns></returns>
        public void ReadVector(Vector3 value)
        {
            Write(value.X);
            Write(value.Y);
            Write(value.Z);
        }
        
        /// <summary>
        /// Special override for strings, which use 2 byte lengths instead of 4
        /// and are null terminated
        /// </summary>
        /// <returns></returns>
        public void WriteGameString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Write<short>(2); // Length
                
                Write<byte>(0); // Null terminator
                Write<byte>(0);
            }
            else
            {
                value += "\0";
                
                // Convert to bytes
                var arr = value.ToCharArray();
                
                // Write the size as a short
                Write<short>((short)(value.Length*2));
                
                // Add to list
                foreach (var c in arr)
                {
                    Write(c);
                }
            }
        }
        
        #region OBJECTS

        /// <summary>
        /// Writes a pilot info object into the packet
        /// </summary>
        /// <param name="info"></param>
        public void WritePilotInfo(PilotInfo info)
        {
            Write(info.AbilityPointsAvailable);
            
            Write((byte)info.HpLevel);
            Write((byte)info.MoveSpeedLevel);
            Write((byte)info.EnLevel);
            Write((byte)info.ScanRangeLevel);
            Write((byte)info.SpLevel);
            Write((byte)info.AimLevel);
            
            Write((byte)0); // Unknown
            Write((byte)0); // Unknown
            
            Write(info.MeleeLevel);
            Write(info.RangedLevel);
            Write(info.SiegeLevel);
            Write(info.RocketLevel);
            
            //packet.WriteInt(0); // Unknown
            Write((byte)0); // NOTE: Not sure why everything after here is offset by one, so i am just doing 3 bytes instead of 4 here
            Write((byte)0);
            Write((byte)0);
        }

        /// <summary>
        /// Writes a part into the packet
        /// </summary>
        /// <param name="part"></param>
        public void WritePartInfo(PartRecord part)
        {
            if (part != null)
            {
                Write(part.Id);
                Write(part.TemplateId);

                Write((ushort) 0x86a0); // Parameter - Possibly for permanent / cannot delete items?
                Write((ushort) 0); // Type? - Zero in packets

                Write(part.Color.R);
                Write(part.Color.G);
                Write(part.Color.B);

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

                Write((byte) partsType); // Looks like 0 indexed version of part type 
                Write((byte) 0); // Unknown

                Write(0); // Unknown

                // FArray - Unknown
                Write(0); // Array size

                // Array contents would go here

                Write(-1); // Expiry time / current durability
                Write(0x14997000); // Max Durability - Value from server packet capture
                Write(2); // If 1, durability, if 2, expired?
                Write(0); // Unknown
            }
            else
            {
                Write(-1);
                Write(0);

                Write((ushort)0);
                Write((ushort)0);

                Write((byte)0);
                Write((byte)0);
                Write((byte)0);

                Write((byte)0); // Unknown
                Write((byte)0); // Unknown

                Write(0); // Unknown

                // FArray - Unknown
                Write(0); // Array size

                // Array contents would go here

                Write(0); // Unknown
                Write(0); // Unknown
                Write(0); // If 1, durability, if 2, expired?
                Write(0); // Unknown
            }
        }
        
        #endregion
    }
}