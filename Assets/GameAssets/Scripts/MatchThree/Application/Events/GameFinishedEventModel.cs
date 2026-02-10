namespace MatchThree.Application.Events
{
    public readonly struct GameFinishedEventModel
    {
        public bool IsWin { get; }

        public int MovesUsed { get; }

        public int TotalMatchGroupCount { get; }

        public int TargetMatchGroupCount { get; }

        public GameFinishedEventModel(bool isWin, int movesUsed, int totalMatchGroupCount, int targetMatchGroupCount)
        {
            IsWin = isWin;
            MovesUsed = movesUsed;
            TotalMatchGroupCount = totalMatchGroupCount;
            TargetMatchGroupCount = targetMatchGroupCount;
        }
    }
}
