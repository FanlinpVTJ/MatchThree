namespace MatchThree.Domain
{
    public readonly struct MoveModel
    {
        public BoardCoordinate FromCoordinate { get; }

        public BoardCoordinate ToCoordinate { get; }

        public MoveModel(BoardCoordinate fromCoordinate, BoardCoordinate toCoordinate)
        {
            FromCoordinate = fromCoordinate;
            ToCoordinate = toCoordinate;
        }
    }
}
