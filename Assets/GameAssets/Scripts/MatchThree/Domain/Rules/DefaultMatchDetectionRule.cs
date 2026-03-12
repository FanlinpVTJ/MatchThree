using System.Collections.Generic;
using MatchThree.Domain.Interfaces;
using MatchThree.Domain.Models;

namespace MatchThree.Domain.Rules
{
    public class DefaultMatchDetectionRule : IMatchDetectionRule
    {
        public List<MatchGroupModel> FindMatchGroupModels(BoardModel boardModel)
        {
            List<MatchGroupModel> matchGroupModels = new List<MatchGroupModel>();

            return matchGroupModels;
        }
    }
}
