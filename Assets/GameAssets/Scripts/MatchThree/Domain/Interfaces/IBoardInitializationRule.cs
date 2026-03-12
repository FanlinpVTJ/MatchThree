using MatchThree.Domain.Models;

namespace MatchThree.Domain.Interfaces
{
    public interface IBoardInitializationRule
    {
        bool IsSatisfied(BoardModel boardModel);
    }
}
