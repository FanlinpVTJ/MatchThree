using System.Collections.Generic;
using MatchThree.Domain.ValueObjects;

namespace MatchThree.Domain.Models
{
    public class BoardModel
    {
        public BoardModel(int widthValue, int heightValue, List<CellModel> cellModels)
        {
            WidthValue = widthValue;
            HeightValue = heightValue;
            CellModels = cellModels;
        }

        public int WidthValue { get; }

        public int HeightValue { get; }

        public List<CellModel> CellModels { get; }

        public bool IsInside(GridPositionModel positionModel)
        {
            return positionModel.RowIndex >= 0 &&
                   positionModel.RowIndex < HeightValue &&
                   positionModel.ColumnIndex >= 0 &&
                   positionModel.ColumnIndex < WidthValue;
        }

        public bool TryGetCellModel(GridPositionModel positionModel, out CellModel cellModel)
        {
            for (int cellIndex = 0; cellIndex < CellModels.Count; cellIndex++)
            {
                CellModel currentCellModel = CellModels[cellIndex];

                if (currentCellModel.PositionModel == positionModel)
                {
                    cellModel = currentCellModel;
                    return true;
                }
            }

            cellModel = null;
            return false;
        }
    }
}
