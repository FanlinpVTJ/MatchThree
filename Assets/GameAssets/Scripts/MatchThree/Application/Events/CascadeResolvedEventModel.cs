namespace MatchThree.Application.Events
{
    public readonly struct CascadeResolvedEventModel
    {
        public int CascadeIndex { get; }

        public int MatchGroupCount { get; }

        public CascadeResolvedEventModel(int cascadeIndex, int matchGroupCount)
        {
            CascadeIndex = cascadeIndex;
            MatchGroupCount = matchGroupCount;
        }
    }
}
