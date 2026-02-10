namespace MatchThree.Application.Events
{
    public readonly struct GameProgressChangedEventModel
    {
        public int MovesUsed { get; }

        public int MovesRemaining { get; }

        public int TotalMatchGroupCount { get; }

        public int TargetMatchGroupCount { get; }

        public GameProgressChangedEventModel(int movesUsed, int movesRemaining, int totalMatchGroupCount, int targetMatchGroupCount)
        {
            MovesUsed = movesUsed;
            MovesRemaining = movesRemaining;
            TotalMatchGroupCount = totalMatchGroupCount;
            TargetMatchGroupCount = targetMatchGroupCount;
        }
    }
}
