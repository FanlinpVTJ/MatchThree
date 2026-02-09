using System.Collections.Generic;

namespace MatchThree.Domain.Contracts
{
    public interface IMatchRule
    {
        List<MatchGroupModel> FindMatches(BoardModel boardModel);
    }
}
