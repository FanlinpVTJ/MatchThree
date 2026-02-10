using System.Collections.Generic;
using MatchThree.Domain;

namespace MatchThree.Application
{
    public readonly struct MoveProcessResultModel
    {
        public bool IsApplied { get; }

        public List<MatchGroupModel> MatchGroups { get; }

        public string ErrorMessage { get; }

        public MoveProcessResultModel(bool isApplied, List<MatchGroupModel> matchGroups, string errorMessage)
        {
            IsApplied = isApplied;
            MatchGroups = matchGroups;
            ErrorMessage = errorMessage;
        }

        public static MoveProcessResultModel Success(List<MatchGroupModel> matchGroups)
        {
            MoveProcessResultModel result = new MoveProcessResultModel(true, matchGroups, string.Empty);

            return result;
        }

        public static MoveProcessResultModel Failed(string errorMessage)
        {
            List<MatchGroupModel> emptyGroups = new List<MatchGroupModel>();
            MoveProcessResultModel result = new MoveProcessResultModel(false, emptyGroups, errorMessage);

            return result;
        }
    }
}
