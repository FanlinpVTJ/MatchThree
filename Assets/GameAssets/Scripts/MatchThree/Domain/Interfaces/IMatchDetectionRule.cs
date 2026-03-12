using System.Collections.Generic;
using MatchThree.Domain.Models;

namespace MatchThree.Domain.Interfaces
{
    public interface IMatchDetectionRule
    {
        List<MatchGroupModel> FindMatchGroupModels(BoardModel boardModel);
    }
}
