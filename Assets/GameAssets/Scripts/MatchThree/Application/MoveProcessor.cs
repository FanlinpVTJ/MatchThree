using System.Collections.Generic;
using MatchThree.Domain;
using MatchThree.Domain.Contracts;

namespace MatchThree.Application
{
    public class MoveProcessor
    {
        private readonly IMoveValidator _moveValidator;

        private readonly IMatchRule _matchRule;

        public MoveProcessor(IMoveValidator moveValidator, IMatchRule matchRule)
        {
            _moveValidator = moveValidator;
            _matchRule = matchRule;
        }

        public MoveProcessResultModel Process(BoardModel boardModel, MoveModel moveModel)
        {
            MoveValidationResultModel validationResult = _moveValidator.Validate(boardModel, moveModel);

            if (!validationResult.IsValid)
            {
                MoveProcessResultModel invalidResult = MoveProcessResultModel.Failed(validationResult.ErrorMessage);

                return invalidResult;
            }

            boardModel.SwapTiles(moveModel.FromCoordinate, moveModel.ToCoordinate);
            List<MatchGroupModel> matchGroups = _matchRule.FindMatches(boardModel);

            if (matchGroups.Count == 0)
            {
                boardModel.SwapTiles(moveModel.FromCoordinate, moveModel.ToCoordinate);

                MoveProcessResultModel noMatchResult = MoveProcessResultModel.Failed("Move does not create a match.");

                return noMatchResult;
            }

            MoveProcessResultModel successResult = MoveProcessResultModel.Success(matchGroups);

            return successResult;
        }
    }
}
