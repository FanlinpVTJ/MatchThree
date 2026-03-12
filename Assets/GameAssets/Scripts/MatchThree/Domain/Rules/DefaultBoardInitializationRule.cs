using System.Collections.Generic;
using MatchThree.Domain.Interfaces;
using MatchThree.Domain.Models;
using MatchThree.Domain.ValueObjects;

namespace MatchThree.Domain.Rules
{
    public class DefaultBoardInitializationRule : IBoardInitializationRule
    {
        public bool IsSatisfied(BoardModel boardModel)
        {
            if (boardModel == null)
            {
                return false;
            }

            if (boardModel.WidthValue <= 0 || boardModel.HeightValue <= 0)
            {
                return false;
            }

            int expectedCellCountValue = boardModel.WidthValue * boardModel.HeightValue;

            if (boardModel.CellModels == null || boardModel.CellModels.Count != expectedCellCountValue)
            {
                return false;
            }

            HashSet<GridPositionModel> occupiedPositionModels = new HashSet<GridPositionModel>();

            for (int cellIndex = 0; cellIndex < boardModel.CellModels.Count; cellIndex++)
            {
                CellModel cellModel = boardModel.CellModels[cellIndex];

                if (boardModel.IsInside(cellModel.PositionModel) == false)
                {
                    return false;
                }

                if (occupiedPositionModels.Add(cellModel.PositionModel) == false)
                {
                    return false;
                }

                if (cellModel.PieceModel == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
