using MatchThree.Domain.ValueObjects;

namespace MatchThree.Domain.Models
{
    public class CellModel
    {
        public CellModel(GridPositionModel positionModel, PieceModel pieceModel)
        {
            PositionModel = positionModel;
            PieceModel = pieceModel;
        }

        public GridPositionModel PositionModel { get; }

        public PieceModel PieceModel { get; private set; }

        public void UpdatePieceModel(PieceModel pieceModel)
        {
            PieceModel = pieceModel;
        }
    }
}
