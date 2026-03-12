using System;
using MatchThree.Domain.Interfaces;
using MatchThree.Domain.Models;

namespace MatchThree.Domain.Rules
{
    public class DefaultSwapValidationRule : ISwapValidationRule
    {
        public SwapValidationResultModel Validate(BoardModel boardModel, SwapCommandModel swapCommandModel)
        {
            if (boardModel == null)
            {
                return new SwapValidationResultModel(false, "Board is not initialized.");
            }

            if (swapCommandModel == null)
            {
                return new SwapValidationResultModel(false, "Swap command is missing.");
            }

            if (boardModel.IsInside(swapCommandModel.SourcePositionModel) == false)
            {
                return new SwapValidationResultModel(false, "Source position is outside of board.");
            }

            if (boardModel.IsInside(swapCommandModel.TargetPositionModel) == false)
            {
                return new SwapValidationResultModel(false, "Target position is outside of board.");
            }

            if (swapCommandModel.SourcePositionModel == swapCommandModel.TargetPositionModel)
            {
                return new SwapValidationResultModel(false, "Swap positions should be different.");
            }

            int rowDistanceValue = Math.Abs(swapCommandModel.SourcePositionModel.RowIndex - swapCommandModel.TargetPositionModel.RowIndex);
            int columnDistanceValue = Math.Abs(swapCommandModel.SourcePositionModel.ColumnIndex - swapCommandModel.TargetPositionModel.ColumnIndex);

            if (rowDistanceValue + columnDistanceValue != 1)
            {
                return new SwapValidationResultModel(false, "Swap positions should be adjacent.");
            }

            if (boardModel.TryGetCellModel(swapCommandModel.SourcePositionModel, out CellModel sourceCellModel) == false)
            {
                return new SwapValidationResultModel(false, "Source cell is missing.");
            }

            if (boardModel.TryGetCellModel(swapCommandModel.TargetPositionModel, out CellModel targetCellModel) == false)
            {
                return new SwapValidationResultModel(false, "Target cell is missing.");
            }

            if (sourceCellModel.PieceModel == null || targetCellModel.PieceModel == null)
            {
                return new SwapValidationResultModel(false, "Swap requires two pieces.");
            }

            return new SwapValidationResultModel(true, string.Empty);
        }
    }
}
