using System;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// I think this contains the health of various destroyable structures and objects
    /// </summary>
    public class GeoObjectsHp : ServerBasePacket
    {
        public override string GetType()
        {
            return "GEOOBJECTS_HP";
        }

        public override byte GetId()
        {
            return 0x6a;
        }

        protected override void WriteImpl()
        {
            // Console.WriteLine("Sending geo obj hp");
            WriteInt(249); // Array size

            for (var i = 0; i < 249; i++)
            {
                if (i == 23 || i == 24 || i == 25)
                    WriteInt(-1);
                else
                    WriteInt(200); // Obj info of some sort?
            }
        }
    }
}