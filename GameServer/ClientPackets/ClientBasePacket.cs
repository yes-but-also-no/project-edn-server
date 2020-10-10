using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Packets;
using GameServer.ServerPackets;
using Console = Colorful.Console;

namespace GameServer.ClientPackets
{
    /// <summary>
    /// Base class for all packets received from client
    /// </summary>
    public abstract class ClientBasePacket : BasePacket
    {
        /// <summary>
        /// Raw packet received from client
        /// </summary>
        protected readonly byte[] _raw;


        private bool _debug;
        /// <summary>
        /// Set to true to debug this packet
        /// </summary>
        protected bool debug {
            set
            {
                // Debug prints
                Console.WriteLine($"[{GetType()}] Packet Read Started", Color.Lime);
                
                Console.WriteLine("Packet size: {0}",Color.Coral, Size);
            
                Console.WriteLine("Packet raw: {0}", Color.Coral,
                    String.Join(" - ", _raw.Select(b => b.ToString("X2")).ToArray()));
                
                _debug = value;
            }}
        
        /// <summary>
        /// The ID of this packet
        /// </summary>
        public byte Id => _raw[2];
        
        /// <summary>
        /// The size of this packet
        /// </summary>
        public short Size => (short)((_raw[1] << 8) + _raw[0]);

        /// <summary>
        /// Position within the packet we have read to
        /// </summary>
        private int _index;
        
        protected ClientBasePacket(byte[] data, GameSession client) : 
            base(client)
        {
            _raw = data;
            
            // Skip header
            _index = 3;
        }

        /// <summary>
        /// Runs the packet from the client based on priority
        /// </summary>
        public void Run()
        {
            if (HighPriority)
                RunSync();
            else
                RunAsync();
        }
        
        /// <summary>
        /// Runs the packet sent by the client syncronysly
        /// </summary>
        public void RunSync()
        {
            try
            {
                RunImpl();
            }
            catch (Exception e)
            {
                // TODO: Implement logging with levels (sever, info, etc)
                var user = GetClient().GetUserName();
                Console.WriteLine("WARNING: User {0} caused error on packet:run {1} - {2}", user, GetType(), e);
            }
        }

        /// <summary>
        /// Runs the packet in another thread (for low priority packets)
        /// </summary>
        public void RunAsync()
        {
            Task.Run(() =>
            {
                try
                {
                    RunImpl();
                }
                catch (Exception e)
                {
                    // TODO: Implement logging with levels (sever, info, etc)
                    var user = GetClient().GetUserName();
                    Console.WriteLine("WARNING: User {0} caused error on packet:run {1} - {2}", user, GetType(), e);
                }
            });
        }

        

        /// <summary>
        /// Runs the packet received from the client
        /// This is only ran once per packet instance, even if the packet is multicasted
        /// </summary>
        protected abstract void RunImpl();

        /// <summary>
        /// Sends a response to this packet
        /// </summary>
        /// <param name="msg"></param>
        protected void SendPacket(ServerBasePacket msg)
        {
            GetClient().SendPacket(msg);
        }
        
        #region READ FUNCTIONS
        
        /// <summary>
        /// Gets a byte from the packet
        /// </summary>
        /// <returns>byte</returns>
        protected byte GetByte()
        {
            var result = _raw[_index];
            _index += 1;
            
            // Debug prints
            if (_debug) Console.WriteLine($"[{GetType()}] Read byte: {result} | 0x{result:x2}", Color.Lime);
            
            return result;
        }
        
        /// <summary>
        /// Gets a bool from the packet
        /// </summary>
        /// <returns>byte</returns>
        protected bool GetBool()
        {
            var result = _raw[_index];
            _index += 1;
            
            // Debug prints
            if (_debug) Console.WriteLine($"[{GetType()}] Read bool: {result}", Color.Lime);
            
            return result == 1;
        }
        
        /// <summary>
        /// Gets a signed short from the buffer
        /// </summary>
        /// <returns></returns>
        protected short GetShort()
        {
            var result = BitConverter.ToInt16(_raw, _index);
            _index += 2;
            
            // Debug prints
            if (_debug) Console.WriteLine($"[{GetType()}] Read short {result} | (unsigned) {(ushort)result}", Color.Lime);
            
            return result;
        }
        
        /// <summary>
        /// Gets an unsigned short from the buffer
        /// </summary>
        /// <returns></returns>
        protected ushort GetUShort()
        {
            var result = BitConverter.ToUInt16(_raw, _index);
            _index += 2;
            
            // Debug prints
            if (_debug) Console.WriteLine($"[{GetType()}] Read ushort {result} | (signed) {(short)result}", Color.Lime);
            
            return result;
        }
        
        /// <summary>
        /// Gets a signed int from the buffer
        /// </summary>
        /// <returns></returns>
        protected int GetInt()
        {
            var result = BitConverter.ToInt32(_raw, _index);
            _index += 4;
            
            // Debug prints
            if (_debug) Console.WriteLine($"[{GetType()}] Read int {result} | (unsigned) {(uint)result}", Color.Lime);
            
            return result;
        }
        
        /// <summary>
        /// Gets an unsigned int from the buffer
        /// </summary>
        /// <returns></returns>
        protected uint GetUInt()
        {
            var result = BitConverter.ToUInt32(_raw, _index);
            _index += 4;
            
            // Debug prints
            if (_debug) Console.WriteLine($"[{GetType()}] Read uint {result} | (signed) {(int)result}", Color.Lime);
            
            return result;
        }

        /// <summary>
        /// Gets a float from the buffer
        /// </summary>
        /// <returns></returns>
        protected float GetFloat()
        {
            var result = BitConverter.ToSingle(_raw, _index);
            _index += 4;
            
            // Debug prints
            if (_debug) Console.WriteLine($"[{GetType()}] Read float {result} | (int) {(int)result}", Color.Lime);
            
            return result;
        }
        
        /// <summary>
        /// Gets a signed long from the buffer
        /// </summary>
        /// <returns></returns>
        protected long GetLong()
        {
            var result = BitConverter.ToInt64(_raw, _index);
            _index += 8;
            return result;
        }
        
        /// <summary>
        /// Gets a string from the buffer
        /// Reads a two byte size, then reads that many unicode characters into a string
        /// </summary>
        /// <returns></returns>
        protected string GetString()
        {
            var length = GetShort();
            return GetString(length);
        }

        /// <summary>
        /// Gets a string from the buffer, specified by a size
        /// If we ever encounter fixed size strings, this will need to be made protected
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private string GetString(int size)
        {
            var newArray = new byte[size];
            Array.Copy(_raw, _index, newArray, 0, size);
            _index += size;
            
            // Debug prints
            if (_debug) Console.WriteLine($"[{GetType()}] Read string {Encoding.Unicode.GetString(newArray)} | (length) {size}", Color.Lime);
            
            return Encoding.Unicode.GetString(newArray);
        }
        
        #endregion
    }
}