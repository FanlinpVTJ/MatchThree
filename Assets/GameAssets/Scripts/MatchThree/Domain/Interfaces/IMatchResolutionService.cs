using System.Collections.Generic;
using MatchThree.Domain.Models;

namespace MatchThree.Domain.Interfaces
{
    public interface IMatchResolutionService
    {
        BoardModel CreateResolvedBoardModel(BoardModel boardModel, List<MatchGroupModel> matchGroupModels);
    }
}
