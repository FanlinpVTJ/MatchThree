using System.Collections.Generic;
using MatchThree.Domain.Contracts;

namespace MatchThree.Domain.Rules
{
    public class PossibleMoveFinder : IPossibleMoveFinder
    {
        private readonly IMoveValidator _moveValidator;

        private readonly IMatchRule _matchRule;

        public PossibleMoveFinder(IMoveValidator moveValidator, IMatchRule matchRule)
        {
            _moveValidator = moveValidator;
            _matchRule = matchRule;
        }

        public bool TryFindAnyMove(BoardModel boardModel, out MoveModel moveModel)
        {
            for (int row = 0; row < boardModel.Height; row++)
            {
                for (int column = 0; column < boardModel.Width; column++)
                {
                    BoardCoordinate fromCoordinate = new BoardCoordinate(column, row);

                    if (TryFindMoveInDirection(boardModel, fromCoordinate, column + 1, row, out moveModel))
                    {
                        return true;
                    }

                    if (TryFindMoveInDirection(boardModel, fromCoordinate, column, row + 1, out moveModel))
                    {
                        return true;
                    }
                }
            }

            MoveModel emptyMoveModel = new MoveModel(new BoardCoordinate(0, 0), new BoardCoordinate(0, 0));
            moveModel = emptyMoveModel;

            return false;
        }

        private bool TryFindMoveInDirection(BoardModel boardModel, BoardCoordinate fromCoordinate, int toColumn, int toRow, out MoveModel moveModel)
        {
            BoardCoordinate toCoordinate = new BoardCoordinate(toColumn, toRow);

            if (!boardModel.IsCoordinateInBounds(toCoordinate))
            {
                MoveModel emptyMoveModel = new MoveModel(new BoardCoordinate(0, 0), new BoardCoordinate(0, 0));
                moveModel = emptyMoveModel;

                return false;
            }

            MoveModel candidateMoveModel = new MoveModel(fromCoordinate, toCoordinate);
            MoveValidationResultModel validationResult = _moveValidator.Validate(boardModel, candidateMoveModel);

            if (!validationResult.IsValid)
            {
                MoveModel emptyMoveModel = new MoveModel(new BoardCoordinate(0, 0), new BoardCoordinate(0, 0));
                moveModel = emptyMoveModel;

                return false;
            }

            boardModel.SwapTiles(fromCoordinate, toCoordinate);
            List<MatchGroupModel> matchGroups = _matchRule.FindMatches(boardModel);
            boardModel.SwapTiles(fromCoordinate, toCoordinate);

            if (matchGroups.Count == 0)
            {
                MoveModel emptyMoveModel = new MoveModel(new BoardCoordinate(0, 0), new BoardCoordinate(0, 0));
                moveModel = emptyMoveModel;

                return false;
            }

            moveModel = candidateMoveModel;

            return true;
        }
    }
}
