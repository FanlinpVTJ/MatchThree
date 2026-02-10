using MatchThree.Domain.Contracts;

namespace MatchThree.Domain.Rules
{
    public class AdjacentMoveValidator : IMoveValidator
    {
        public MoveValidationResultModel Validate(BoardModel boardModel, MoveModel moveModel)
        {
            if (!boardModel.IsCoordinateInBounds(moveModel.FromCoordinate))
            {
                MoveValidationResultModel invalidFromCoordinateResult = MoveValidationResultModel.Invalid("From coordinate is out of board bounds.");

                return invalidFromCoordinateResult;
            }

            if (!boardModel.IsCoordinateInBounds(moveModel.ToCoordinate))
            {
                MoveValidationResultModel invalidToCoordinateResult = MoveValidationResultModel.Invalid("To coordinate is out of board bounds.");

                return invalidToCoordinateResult;
            }

            if (moveModel.FromCoordinate.Column == moveModel.ToCoordinate.Column
                && moveModel.FromCoordinate.Row == moveModel.ToCoordinate.Row)
            {
                MoveValidationResultModel sameCoordinateResult = MoveValidationResultModel.Invalid("From and to coordinates are the same.");

                return sameCoordinateResult;
            }

            int columnDistance = moveModel.FromCoordinate.Column - moveModel.ToCoordinate.Column;

            if (columnDistance < 0)
            {
                columnDistance = -columnDistance;
            }

            int rowDistance = moveModel.FromCoordinate.Row - moveModel.ToCoordinate.Row;

            if (rowDistance < 0)
            {
                rowDistance = -rowDistance;
            }

            int manhattanDistance = columnDistance + rowDistance;

            if (manhattanDistance != 1)
            {
                MoveValidationResultModel distanceResult = MoveValidationResultModel.Invalid("Move must target an adjacent cell.");

                return distanceResult;
            }

            CellModel fromCell = boardModel.GetCell(moveModel.FromCoordinate);
            CellModel toCell = boardModel.GetCell(moveModel.ToCoordinate);

            if (fromCell.IsBlocked || toCell.IsBlocked)
            {
                MoveValidationResultModel blockedCellResult = MoveValidationResultModel.Invalid("Move cannot involve blocked cells.");

                return blockedCellResult;
            }

            if (fromCell.IsEmpty || toCell.IsEmpty)
            {
                MoveValidationResultModel emptyCellResult = MoveValidationResultModel.Invalid("Move cannot involve empty cells.");

                return emptyCellResult;
            }

            MoveValidationResultModel validResult = MoveValidationResultModel.Valid();

            return validResult;
        }
    }
}
