using MatchThree.Domain;

namespace MatchThree.Application.Events
{
    public readonly struct MoveStartedEventModel
    {
        public MoveModel MoveModel { get; }

        public MoveStartedEventModel(MoveModel moveModel)
        {
            MoveModel = moveModel;
        }
    }
}
