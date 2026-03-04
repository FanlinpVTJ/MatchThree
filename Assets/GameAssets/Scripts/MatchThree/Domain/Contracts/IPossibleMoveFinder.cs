namespace MatchThree.Domain.Contracts
{
    public interface IPossibleMoveFinder
    {
        bool TryFindAnyMove(BoardModel boardModel, out MoveModel moveModel);
    }
}
