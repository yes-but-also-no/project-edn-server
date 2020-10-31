using System;
using System.IO;
using System.Numerics;
using GameServer.Util;
using Console = Colorful.Console;

namespace GameServer.GeoEngine
{
    /// <summary>
    /// Test class for decoding map data
    /// </summary>
    public class Map
    {
        private readonly int MapMinX;
        private readonly int MapMaxX;
        
        private readonly int MapMinY;
        private readonly int MapMaxY;
        
        private readonly int MapMinZ;
        private readonly int MapMaxZ;
        
        public int BlocksX;
        public int BlocksY;
        public int MapBlocks => BlocksX * BlocksY;

        private readonly Block[] _blocks;

        public Map(BinaryReader reader)
        {
            // Read map size?
            MapMinX = ByteReader.GetInt(reader);
            MapMaxX = ByteReader.GetInt(reader);
            
            MapMinY = ByteReader.GetInt(reader);
            MapMaxY = ByteReader.GetInt(reader);
            
            MapMinZ = ByteReader.GetInt(reader);
            MapMaxZ = ByteReader.GetInt(reader);
            
            // Calculate size
            var difB = MapMaxY - MapMinY;
            var difA = MapMaxX - MapMinX;

            // Calculate number of rows and columns
            BlocksX = difB >> 7;
            BlocksY = difA >> 7;
            
            // Create array
            _blocks = new Block[MapBlocks];

            for (var blockOffset = 0; blockOffset < MapBlocks; blockOffset++)
            {
                var blockSize = ByteReader.GetInt(reader);
                var blockType = ByteReader.GetByte(reader);

                switch (blockType)
                {
                    case Block.TYPE_FLAT:
                        _blocks[blockOffset] = new FlatBlock(reader);
                        break;
                    
                    case Block.TYPE_COMPLEX:
                        _blocks[blockOffset] = new ComplexBlock(reader);
                        break;
                    
                    case Block.TYPE_MULTILAYER:
                        // This one needs size
                        _blocks[blockOffset] = new MultilayerBlock(reader, blockSize);
                        break;
                    
                    default:
                        throw new Exception($"Invalid block type: {blockType}!");
                }
                
                // Read extra bs
                var otherSize = ByteReader.GetInt(reader);

                for (var d = 0; d < otherSize; d++)
                {
                    ByteReader.GetInt(reader);
                }
            }
            
            // TODO: Building data and stuff would go here
        }

        private Block _getBlock(int geoX, int geoY)
        {
            return _blocks[(((geoX / Block.BLOCK_CELLS_X) % BlocksX) * BlocksY) + ((geoY / Block.BLOCK_CELLS_Y) % BlocksY)];
        }
        
        public bool CheckNearest(int geoX, int geoY, int worldZ) {
            return _getBlock(geoX, geoY).CheckNearest(geoX, geoY, worldZ);
        }
        
        public int GetNearestZ(int geoX, int geoY, int worldZ) {
            return _getBlock(geoX, geoY).GetNearestZ(geoX, geoY, worldZ);
        }
        
        public int GetNextLowerZ(int geoX, int geoY, int worldZ) {
            return _getBlock(geoX, geoY).GetNextLowerZ(geoX, geoY, worldZ);
        }
        
        public int GetNextHigherZ(int geoX, int geoY, int worldZ) {
            return _getBlock(geoX, geoY).GetNextHigherZ(geoX, geoY, worldZ);
        }

        public int GetGeoX(int worldX)
        {
            return (worldX - MapMinX) / 16;
        }
        
        public int GetGeoY(int worldY)
        {
            return (worldY - MapMinY) / 16;
        }
        
        public (int, int, int) GetGeoXYZ(Vector3 world)
        {
            return (GetGeoX((int) world.X), GetGeoY((int) world.Y), (int) world.Z);
        }

        public int GetWorldX(int geoX)
        {
            return (geoX * 16) + MapMinX + 8;
        }
        
        public int GetWorldY(int geoY)
        {
            return (geoY * 16) + MapMinY + 8;
        }
        
        private const int ELEVATED_SEE_OVER_DISTANCE = 2;
	
        private const int MAX_SEE_OVER_HEIGHT = 48;
        
        // TODO: Move these to new class? Or abstraction?
        /// <summary>
        /// Checks for movement between two positions, stopping at the first geo wall encountered
        /// TODO: Similar for blade / rocket attacks?
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public Vector3 MoveCheck(Vector3 start, Vector3 end)
        {
            // Convert world pos to geo
            var (geoX, geoY, z) = GetGeoXYZ(start);

            // Get nearest z layer
            z = GetNearestZ(geoX, geoY, z);
            
            // Get target pos as geo
            var (tGeoX, tGeoY, tz) = GetGeoXYZ(end);

            // Get nearest z layer
            tz = GetNearestZ(tGeoX, tGeoY, tz);
            
            // TODO: Check for collisions with geoobjects

            var pointIter = new LinePointIterator(geoX, geoY, tGeoX, tGeoY);
            
            // First point is always available
            pointIter.Next();

            var prevX = pointIter.X();
            var prevY = pointIter.Y();
            var prevZ = z;

            // Check blocks between
            while (pointIter.Next())
            {
                var curX = pointIter.X();
                var curY = pointIter.Y();
                var curZ = GetNearestZ(curX, curY, prevZ);
                
                // Check for can move
                if (!CheckNearest(curX, curY, curZ))
                {
                    return new Vector3(GetWorldX(prevX), GetWorldY(prevY), prevZ);
                }

                prevX = curX;
                prevY = curY;
                prevZ = curZ;
            }
            
            // Skip floor check?
            
            return end;
        }

        public Vector3 MoveCheck3D(Vector3 start, Vector3 end)
        {
            // Convert world pos to geo
            var (geoX, geoY, z) = GetGeoXYZ(start);
            
            // Get target pos as geo
            var (tGeoX, tGeoY, tz) = GetGeoXYZ(end);
            
            var pointIter = new LinePointIterator3D(geoX, geoY, z, tGeoX, tGeoY, tz);
            
            // First is garaunteed
            pointIter.Next();

            var prevX = pointIter.X();
            var prevY = pointIter.Y();
            var prevZ = pointIter.Z();
            var prevGeoZ = GetNextLowerZ(prevX, prevY, prevZ);
            int ptIndex = 0;
            // Are we attacking or shooting downwards? So we know if we need to check passthrough
            var isGoingDownwards = tz < z;

            while (pointIter.Next())
            {
                var curX = pointIter.X();
                var curY = pointIter.Y();

                if ((curX == prevX) && (curY == prevY))
                {
                    continue;
                }

                var curZ = pointIter.Z();
                
                // Use lower Z to handle 3d layers
                var curGeoZ = GetNextLowerZ(curX, curY, curZ);

                if (curGeoZ < prevGeoZ)
                {
                    // If there is a layer drop, make sure we are not clipping the layer above
                    curGeoZ = GetNearestZ(curX, curY, prevGeoZ);
                }

                var canSeeThrough = false;
                
                // If we are trying to pass through a block
                if (isGoingDownwards && curZ <= curGeoZ)
                {
                    return new Vector3(GetWorldX(prevX), GetWorldY(prevY), curGeoZ);
                }
                
                if (CheckNearest(curX, curY, curZ))
                {
                    // Skipping the diagnol checks??
                    canSeeThrough = true;
                    //Console.WriteLine($"[{ptIndex}] - Result: {canSeeThrough}");
                }

                if (!canSeeThrough)
                {
                    return new Vector3(GetWorldX(prevX), GetWorldY(prevY), prevZ);
                }

                prevX = curX;
                prevY = curY;
                prevZ = curZ;
                prevGeoZ = curGeoZ;
                ++ptIndex;
            }

            return end;
        }
    }
}