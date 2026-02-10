using System.Collections.Generic;
using MatchThree.Application.Events;
using MatchThree.Domain;
using MatchThree.Domain.Contracts;
using MessagePipe;

namespace MatchThree.Application
{
    public class MoveProcessor
    {
        private readonly IMoveValidator _moveValidator;

        private readonly CascadeProcessor _cascadeProcessor;

        private readonly IPublisher<MoveStartedEventModel> _moveStartedEventPublisher;

        private readonly IPublisher<MoveRejectedEventModel> _moveRejectedEventPublisher;

        private readonly IPublisher<MoveAppliedEventModel> _moveAppliedEventPublisher;

        private readonly IPublisher<BoardSettledEventModel> _boardSettledEventPublisher;

        public MoveProcessor(
            IMoveValidator moveValidator,
            CascadeProcessor cascadeProcessor,
            IPublisher<MoveStartedEventModel> moveStartedEventPublisher,
            IPublisher<MoveRejectedEventModel> moveRejectedEventPublisher,
            IPublisher<MoveAppliedEventModel> moveAppliedEventPublisher,
            IPublisher<BoardSettledEventModel> boardSettledEventPublisher)
        {
            _moveValidator = moveValidator;
            _cascadeProcessor = cascadeProcessor;
            _moveStartedEventPublisher = moveStartedEventPublisher;
            _moveRejectedEventPublisher = moveRejectedEventPublisher;
            _moveAppliedEventPublisher = moveAppliedEventPublisher;
            _boardSettledEventPublisher = boardSettledEventPublisher;
        }

        public MoveProcessResultModel Process(BoardModel boardModel, MoveModel moveModel)
        {
            MoveStartedEventModel startedEventModel = new MoveStartedEventModel(moveModel);
            _moveStartedEventPublisher.Publish(startedEventModel);

            MoveValidationResultModel validationResult = _moveValidator.Validate(boardModel, moveModel);

            if (!validationResult.IsValid)
            {
                MoveRejectedEventModel rejectedEventModel = new MoveRejectedEventModel(moveModel, validationResult.ErrorMessage);
                _moveRejectedEventPublisher.Publish(rejectedEventModel);
                MoveProcessResultModel invalidResult = MoveProcessResultModel.Failed(validationResult.ErrorMessage);

                return invalidResult;
            }

            boardModel.SwapTiles(moveModel.FromCoordinate, moveModel.ToCoordinate);
            CascadeResolveResultModel cascadeResult = _cascadeProcessor.Resolve(boardModel);

            if (cascadeResult.MatchGroups.Count == 0)
            {
                boardModel.SwapTiles(moveModel.FromCoordinate, moveModel.ToCoordinate);
                MoveRejectedEventModel rejectedEventModel = new MoveRejectedEventModel(moveModel, "Move does not create a match.");
                _moveRejectedEventPublisher.Publish(rejectedEventModel);

                MoveProcessResultModel noMatchResult = MoveProcessResultModel.Failed("Move does not create a match.");

                return noMatchResult;
            }

            List<MatchGroupModel> matchGroups = cascadeResult.MatchGroups;
            MoveAppliedEventModel appliedEventModel = new MoveAppliedEventModel(moveModel, matchGroups.Count, cascadeResult.CascadeCount);
            _moveAppliedEventPublisher.Publish(appliedEventModel);
            BoardSettledEventModel boardSettledEventModel = new BoardSettledEventModel(matchGroups.Count, cascadeResult.CascadeCount);
            _boardSettledEventPublisher.Publish(boardSettledEventModel);
            MoveProcessResultModel successResult = MoveProcessResultModel.Success(matchGroups);

            return successResult;
        }
    }
}
