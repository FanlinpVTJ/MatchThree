namespace MatchThree.Application.Events
{
    public readonly struct BoardSettledEventModel
    {
        public int TotalMatchGroupCount { get; }

        public int CascadeCount { get; }

        public BoardSettledEventModel(int totalMatchGroupCount, int cascadeCount)
        {
            TotalMatchGroupCount = totalMatchGroupCount;
            CascadeCount = cascadeCount;
        }
    }
}
