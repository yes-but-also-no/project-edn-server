using System;
using System.IO;

namespace GameServer.GeoEngine
{
    public class MultilayerBlock : Block
    {
        private readonly short[] _heightData;
        private readonly short[] _flagData;
        
        private readonly short[] _offsetData;

        public MultilayerBlock(BinaryReader reader, int size)
        {
            Size = size;
            
            _heightData = new short[Size];
            _flagData = new short[Size];
            
            _offsetData = new short[BLOCK_CELLS];
            
            // Read cell data
            for (var cellOffset = 0; cellOffset < Size; cellOffset++)
            {
                _flagData[cellOffset] = ByteReader.GetShort(reader);
                _heightData[cellOffset] = ByteReader.GetShort(reader);
            }
            
            // Read offset data
            for (var cellOffset = 0; cellOffset < BLOCK_CELLS; cellOffset++)
            {
                _offsetData[cellOffset] = ByteReader.GetShort(reader);
            }
        }
        
        private int _getCellLayers(int geoX, int geoY)
        {           
            // Index to the cell
            var index = ((geoX % BLOCK_CELLS_X) * BLOCK_CELLS_Y) + (geoY % BLOCK_CELLS_Y);
                                
            // Num layers
            var offset = _offsetData[index];
            
            return index == 63 ? Size - offset : _offsetData[index + 1] - offset;
        }
        
        private (short, short) _getCellData(int geoX, int geoY, int layer = 0)
        {
            // Index to the cell
            var index = ((geoX % BLOCK_CELLS_X) * BLOCK_CELLS_Y) + (geoY % BLOCK_CELLS_Y);
                                
            // Num layers
            var offset = _offsetData[index];
                                
            return (_flagData[offset + layer], _heightData[offset + layer]);
        }

        private (short, short) _getNearestLayer(int geoX, int geoY, int worldZ)
        {
            // Index to the cell
            var index = ((geoX % BLOCK_CELLS_X) * BLOCK_CELLS_Y) + (geoY % BLOCK_CELLS_Y);
                                
            // Get offset
            var startOffset = _offsetData[index];
            
            // Calculate num layers
            var numLayers = index == 63 ? Size - startOffset : _offsetData[index + 1] - startOffset;

            int nearestDZ = 0;
            (short, short) nearestData = (0, 0);

            // loop
            for (var i = 0; i < numLayers; i++)
            {
                var layerData = (_flagData[startOffset + i], _heightData[startOffset + i]);

                if (layerData.Item2 == worldZ)
                {
                    // Exact match
                    return layerData;
                }

                var layerDZ = Math.Abs(layerData.Item2 - worldZ);
                if (i == 0 || (layerDZ < nearestDZ))
                {
                    nearestDZ = layerDZ;
                    nearestData = layerData;
                }
                
            }

            return nearestData;
        }
        
        private (short, short) _getNextLowerLayer(int geoX, int geoY, int worldZ)
        {
            // Index to the cell
            var index = ((geoX % BLOCK_CELLS_X) * BLOCK_CELLS_Y) + (geoY % BLOCK_CELLS_Y);
                                
            // Get offset
            var startOffset = _offsetData[index];
            
            // Calculate num layers
            var numLayers = index == 63 ? Size - startOffset : _offsetData[index + 1] - startOffset;

            int lowerZ = Int32.MinValue;
            (short, short) nearestData = (0, 0);

            // loop
            for (var i = 0; i < numLayers; i++)
            {
                var layerData = (_flagData[startOffset + i], _heightData[startOffset + i]);

                if (layerData.Item2 == worldZ)
                {
                    // Exact match
                    return layerData;
                }

                if ((layerData.Item2 < worldZ) && (layerData.Item2 > lowerZ))
                {
                    lowerZ = layerData.Item2;
                    nearestData = layerData;
                }
                
            }

            return nearestData;
        }

        public override bool CheckNearest(int geoX, int geoY, int worldZ)
        {
//            return canMove(_getNearestLayer(geoX, geoY, worldZ).Item1);
            // Switch to lower layer instead of nearest because this game is 3d
            return canMove(_getNextLowerLayer(geoX, geoY, worldZ).Item1);
        }

        public override int GetNearestZ(int geoX, int geoY, int worldZ)
        {
            return _getNearestLayer(geoX, geoY, worldZ).Item2;
        }

        public override int GetNextLowerZ(int geoX, int geoY, int worldZ)
        {
            // Index to the cell
            var index = ((geoX % BLOCK_CELLS_X) * BLOCK_CELLS_Y) + (geoY % BLOCK_CELLS_Y);

            // Get offset
            var startOffset = _offsetData[index];

            // Calculate num layers
            var numLayers = index == 63 ? Size - startOffset : _offsetData[index + 1] - startOffset;

            int lowerZ = Int32.MinValue;

            // loop
            for (var i = 0; i < numLayers; i++)
            {
                var layerData = (_flagData[startOffset + i], _heightData[startOffset + i]);

                if (layerData.Item2 == worldZ)
                {
                    // Exact match
                    return layerData.Item2;
                }

                if ((layerData.Item2 < worldZ) && (layerData.Item2 > lowerZ))
                {
                    lowerZ = layerData.Item2;
                }
            }

            return lowerZ == Int32.MinValue ? worldZ : lowerZ;
        }
        
        public override int GetNextHigherZ(int geoX, int geoY, int worldZ)
        {
            // Index to the cell
            var index = ((geoX % BLOCK_CELLS_X) * BLOCK_CELLS_Y) + (geoY % BLOCK_CELLS_Y);

            // Get offset
            var startOffset = _offsetData[index];

            // Calculate num layers
            var numLayers = index == 63 ? Size - startOffset : _offsetData[index + 1] - startOffset;

            int higherZ = Int32.MaxValue;

            // loop
            for (var i = 0; i < numLayers; i++)
            {
                var layerData = (_flagData[startOffset + i], _heightData[startOffset + i]);

                if (layerData.Item2 == worldZ)
                {
                    // Exact match
                    return layerData.Item2;
                }

                if ((layerData.Item2 > worldZ) && (layerData.Item2 < higherZ))
                {
                    higherZ = layerData.Item2;
                }
            }

            return higherZ == Int32.MaxValue ? worldZ : higherZ;
        }
    }
}