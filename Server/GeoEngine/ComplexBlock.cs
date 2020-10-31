using System.IO;

namespace GameServer.GeoEngine
{
    public class ComplexBlock : Block
    {
        private readonly short[] _heightData;
        private readonly short[] _flagData;

        public ComplexBlock(BinaryReader reader)
        {
            _heightData = new short[BLOCK_CELLS];
            _flagData = new short[BLOCK_CELLS];
            
            for (var cellOffset = 0; cellOffset < BLOCK_CELLS; cellOffset++)
            {
                _flagData[cellOffset] = ByteReader.GetShort(reader);
                _heightData[cellOffset] = ByteReader.GetShort(reader);
            }
        }

        private (short, short) _getCellData(int geoX, int geoY)
        {
            var index = ((geoX % BLOCK_CELLS_X) * BLOCK_CELLS_Y) + (geoY % BLOCK_CELLS_Y);
            return (_flagData[index], _heightData[index]);
        }

        public override bool CheckNearest(int geoX, int geoY, int worldZ)
        {
            return canMove(_getCellData(geoX, geoY).Item1);
        }

        public override int GetNearestZ(int geoX, int geoY, int worldZ)
        {
            return _getCellData(geoX, geoY).Item2;
        }

        public override int GetNextLowerZ(int geoX, int geoY, int worldZ)
        {
            var cellHeight = _getCellData(geoX, geoY).Item2;
            return cellHeight <= worldZ ? cellHeight : worldZ;
        }
        
        public override int GetNextHigherZ(int geoX, int geoY, int worldZ)
        {
            var cellHeight = _getCellData(geoX, geoY).Item2;
            return cellHeight >= worldZ ? cellHeight : worldZ;
        }
    }
}