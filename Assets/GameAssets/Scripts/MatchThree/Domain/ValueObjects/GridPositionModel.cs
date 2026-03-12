using System;

namespace MatchThree.Domain.ValueObjects
{
    public readonly struct GridPositionModel : IEquatable<GridPositionModel>
    {
        public GridPositionModel(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }

        public int RowIndex { get; }

        public int ColumnIndex { get; }

        public bool Equals(GridPositionModel other)
        {
            return RowIndex == other.RowIndex && ColumnIndex == other.ColumnIndex;
        }

        public override bool Equals(object objectValue)
        {
            if (objectValue is GridPositionModel positionModel)
            {
                return Equals(positionModel);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RowIndex, ColumnIndex);
        }

        public static bool operator ==(GridPositionModel leftPositionModel, GridPositionModel rightPositionModel)
        {
            return leftPositionModel.Equals(rightPositionModel);
        }

        public static bool operator !=(GridPositionModel leftPositionModel, GridPositionModel rightPositionModel)
        {
            return leftPositionModel.Equals(rightPositionModel) == false;
        }
    }
}
