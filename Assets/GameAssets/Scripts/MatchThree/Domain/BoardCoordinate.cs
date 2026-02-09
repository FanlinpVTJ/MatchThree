namespace MatchThree.Domain
{
    public readonly struct BoardCoordinate
    {
        public int Column { get; }

        public int Row { get; }

        public BoardCoordinate(int column, int row)
        {
            Column = column;
            Row = row;
        }
    }
}
