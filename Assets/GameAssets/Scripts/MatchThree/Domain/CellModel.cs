namespace MatchThree.Domain
{
    public class CellModel
    {
        public BoardCoordinate Coordinate { get; }

        public TileModel Tile { get; private set; }

        public bool IsBlocked { get; private set; }

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
        }

        public void SetTile(TileModel tile)
        {
            Tile = tile;
        }

        public void SetBlocked(bool isBlocked)
        {
            IsBlocked = isBlocked;
        }

        public void ClearTile()
        {
            Tile = null;
        }
    }
}
