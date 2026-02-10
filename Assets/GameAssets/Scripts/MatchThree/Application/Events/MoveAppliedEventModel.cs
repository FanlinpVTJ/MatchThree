using MatchThree.Domain;

namespace MatchThree.Application.Events
{
    public readonly struct MoveAppliedEventModel
    {
        public MoveModel MoveModel { get; }

        public int TotalMatchGroupCount { get; }

        public int CascadeCount { get; }

        public MoveAppliedEventModel(MoveModel moveModel, int totalMatchGroupCount, int cascadeCount)
        {
            MoveModel = moveModel;
            TotalMatchGroupCount = totalMatchGroupCount;
            CascadeCount = cascadeCount;
        }
    }
}
