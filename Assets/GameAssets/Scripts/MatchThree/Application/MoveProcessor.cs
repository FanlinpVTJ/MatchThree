using System.Collections.Generic;
using MatchThree.Domain;
using MatchThree.Domain.Contracts;

namespace MatchThree.Application
{
    public class MoveProcessor
    {
        private readonly IMoveValidator _moveValidator;

        private readonly CascadeProcessor _cascadeProcessor;

        public MoveProcessor(IMoveValidator moveValidator, CascadeProcessor cascadeProcessor)
        {
            _moveValidator = moveValidator;
            _cascadeProcessor = cascadeProcessor;
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
            CascadeResolveResultModel cascadeResult = _cascadeProcessor.Resolve(boardModel);

            if (cascadeResult.MatchGroups.Count == 0)
            {
                boardModel.SwapTiles(moveModel.FromCoordinate, moveModel.ToCoordinate);

                MoveProcessResultModel noMatchResult = MoveProcessResultModel.Failed("Move does not create a match.");

                return noMatchResult;
            }

            List<MatchGroupModel> matchGroups = cascadeResult.MatchGroups;
            MoveProcessResultModel successResult = MoveProcessResultModel.Success(matchGroups);

            return successResult;
        }
    }
}
