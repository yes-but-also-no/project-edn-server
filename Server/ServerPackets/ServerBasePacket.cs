using System;
using System.Collections.Generic;
using System.Linq;
using Data.Packets;

namespace GameServer.ServerPackets
{
    /// <summary>
    /// Base class for all packets sent from the server
    /// </summary>
    public abstract class ServerBasePacket : BasePacket
    {
        /// <summary>
        /// List of data to be sent back
        /// </summary>
        private readonly List<byte> _data = new List<byte>();

        /// <summary>
        /// Id of this packet
        /// </summary>
        public abstract byte GetId();
        
        protected ServerBasePacket() { }
        
        protected ServerBasePacket(GameSession client) : base(client)
        {
            
        }
        
        /// <summary>
        /// Compiles the packet into a byte array for sending
        /// </summary>
        /// <returns></returns>
        public byte[] Write()
        {
            // Call packet write implementation first
            WriteImpl();
            
            // Account for header
            var size = _data.Count + 3;
            
            // Add size and header
            _data.Insert(0, GetId());
            _data.InsertRange(0, BitConverter.GetBytes((short)size));
            
            return _data.ToArray();
        }

        /// <summary>
        /// Called when the system is ready to send this packet
        /// </summary>
        protected abstract void WriteImpl();
        //protected abstract void RunImpl();
        
        #region WRITE FUNCTIONS
        
        /// <summary>
        /// Writes a raw byte array into the buffer
        /// </summary>
        /// <param name="data"></param>
        public void WriteRaw(byte[] data)
        {
            _data.AddRange(data);
        }

        /// <summary>
        /// Writes a single byte
        /// </summary>
        /// <param name="data"></param>
        public void WriteByte(byte data)
        {
            _data.Add(data);
        }

        /// <summary>
        /// Writes a bool to a single byte
        /// </summary>
        /// <param name="data"></param>
        public void WriteBool(bool data)
        {
            _data.Add((byte)(data ? 0x1 : 0x0));
        }

        /// <summary>
        /// Writes an int
        /// </summary>
        /// <param name="data"></param>
        public void WriteInt(int data)
        {
            _data.AddRange(BitConverter.GetBytes(data));
        }
        
        /// <summary>
        /// Writes a uint
        /// </summary>
        /// <param name="data"></param>
        public void WriteUInt(uint data)
        {
            _data.AddRange(BitConverter.GetBytes(data));
        }

        /// <summary>
        /// Writes a short
        /// </summary>
        /// <param name="data"></param>
        public void WriteShort(short data)
        {
            _data.AddRange(BitConverter.GetBytes(data));
        }
        
        /// <summary>
        /// Writes a ushort
        /// </summary>
        /// <param name="data"></param>
        public void WriteUShort(ushort data)
        {
            _data.AddRange(BitConverter.GetBytes(data));
        }

        /// <summary>
        /// Writes a long
        /// </summary>
        /// <param name="data"></param>
        public void WriteLong(long data)
        {
            _data.AddRange(BitConverter.GetBytes(data));
        }

        /// <summary>
        /// Writes a float
        /// </summary>
        /// <param name="data"></param>
        public void WriteFloat(float data)
        {
            _data.AddRange(BitConverter.GetBytes(data));
        }
        
        /// <summary>
        /// Writes a string of text, prepended by its length
        /// </summary>
        /// <param name="text"></param>
        public void WriteString(string text, bool nullTerminate = true)
        {
            // Not sure how to handle this so...
            if (String.IsNullOrEmpty(text))
            {
                _data.AddRange(new byte[]{0x02, 0x00, 0x00, 0x00});
            }
            else
            {
                if (nullTerminate) text += "\0";
                
                // Convert to bytes
                var arr = text.ToCharArray();
                
                // Write the size as a short
                _data.AddRange(BitConverter.GetBytes((short)(text.Length*2)));
                
                // Add to list
                foreach (var c in arr)
                {
                    _data.AddRange(BitConverter.GetBytes(c));
                }
            }
        }
        
        #endregion
    }
}