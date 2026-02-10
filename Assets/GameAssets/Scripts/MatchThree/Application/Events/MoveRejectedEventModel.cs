using MatchThree.Domain;

namespace MatchThree.Application.Events
{
    public readonly struct MoveRejectedEventModel
    {
        public MoveModel MoveModel { get; }

        public string ErrorMessage { get; }

        public MoveRejectedEventModel(MoveModel moveModel, string errorMessage)
        {
            MoveModel = moveModel;
            ErrorMessage = errorMessage;
        }
    }
}
