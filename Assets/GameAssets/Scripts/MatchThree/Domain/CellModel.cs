namespace MatchThree.Domain
{
    public class CellModel
    {
        public BoardCoordinate Coordinate { get; }

        public TileModel Tile { get; private set; }

        public bool IsBlocked { get; private set; }

        public CellBlockType BlockType { get; private set; }

        public int BlockDurability { get; private set; }

        public bool IsEmpty
        {
            get
            {
                return Tile == null;
            }
        }

        public CellModel(BoardCoordinate coordinate, TileModel tile, bool isBlocked)
        {
            Coordinate = coordinate;
            Tile = tile;
            IsBlocked = isBlocked;
            BlockType = isBlocked ? CellBlockType.Solid : CellBlockType.None;
            BlockDurability = 0;
        }

        public void SetTile(TileModel tile)
        {
            Tile = tile;
        }

        public void SetBlocked(CellBlockType blockType, int blockDurability)
        {
            if (blockType == CellBlockType.None)
            {
                IsBlocked = false;
                BlockType = CellBlockType.None;
                BlockDurability = 0;

                return;
            }

            IsBlocked = true;
            BlockType = blockType;
            BlockDurability = blockDurability;
        }

        public bool CanReceiveDamage()
        {
            if (!IsBlocked)
            {
                return false;
            }

            if (BlockType != CellBlockType.Durable)
            {
                return false;
            }

            return BlockDurability > 0;
        }

        public bool ApplyDamage(int damage)
        {
            if (!CanReceiveDamage())
            {
                return false;
            }

            BlockDurability -= damage;

            if (BlockDurability > 0)
            {
                return false;
            }

            SetBlocked(CellBlockType.None, 0);

            return true;
        }

        public void ClearTile()
        {
            Tile = null;
        }
    }
}
