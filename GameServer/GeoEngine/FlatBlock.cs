using System.IO;

namespace GameServer.GeoEngine
{
    public class FlatBlock : Block
    {
        private short _height;
        private short _flags;

        public FlatBlock(BinaryReader reader)
        {
            _flags = ByteReader.GetShort(reader);
            _height = ByteReader.GetShort(reader);
        }

        public override bool CheckNearest(int geoX, int geoY, int worldZ)
        {
            return canMove(_flags);
        }

        public override int GetNearestZ(int geoX, int geoY, int worldZ)
        {
            return _height;
        }

        public override int GetNextLowerZ(int geoX, int geoY, int worldZ)
        {
            return _height <= worldZ ? _height : worldZ;
        }

        public override int GetNextHigherZ(int geoX, int geoY, int worldZ)
        {
            return _height >= worldZ ? _height : worldZ;
        }
    }
}