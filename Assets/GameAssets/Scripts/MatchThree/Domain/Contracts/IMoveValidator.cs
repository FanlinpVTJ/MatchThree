namespace MatchThree.Domain.Contracts
{
    public interface IMoveValidator
    {
        MoveValidationResultModel Validate(BoardModel boardModel, MoveModel moveModel);
    }
}
