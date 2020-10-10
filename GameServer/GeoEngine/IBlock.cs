namespace GameServer.GeoEngine
{   
    public abstract class Block
    {
        public const int TYPE_FLAT = 0x03;
        public const int TYPE_COMPLEX = 0x01;
        public const int TYPE_MULTILAYER = 0x00;

        public const int BLOCK_CELLS_X = 8;
        public const int BLOCK_CELLS_Y = 8;
        public const int BLOCK_CELLS = BLOCK_CELLS_X * BLOCK_CELLS_Y;

        public int Size;

        protected bool canMove(short flags)
        {
            // Note: removed flag 0080 which was saying false on blocks that CAN enter but force you out
            return (flags & 0x0900) == 0x0900;
        }
        
        public abstract bool CheckNearest(int geoX, int geoY, int worldZ);

        public abstract int GetNearestZ(int geoX, int geoY, int worldZ);
        public abstract int GetNextLowerZ(int geoX, int geoY, int worldZ);
        public abstract int GetNextHigherZ(int geoX, int geoY, int worldZ);
    }
}