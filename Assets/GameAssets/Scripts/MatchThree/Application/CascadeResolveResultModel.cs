using System.Collections.Generic;
using MatchThree.Domain;

namespace MatchThree.Application
{
    public readonly struct CascadeResolveResultModel
    {
        public List<MatchGroupModel> MatchGroups { get; }

        public int CascadeCount { get; }

        public CascadeResolveResultModel(List<MatchGroupModel> matchGroups, int cascadeCount)
        {
            MatchGroups = matchGroups;
            CascadeCount = cascadeCount;
        }
    }
}
