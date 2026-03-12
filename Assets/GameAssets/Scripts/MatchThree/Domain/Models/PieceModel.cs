using MatchThree.Domain.ValueObjects;

namespace MatchThree.Domain.Models
{
    public class PieceModel
    {
        public PieceModel(int pieceIdentifier, int pieceTypeIdentifier, GridPositionModel positionModel)
        {
            PieceIdentifier = pieceIdentifier;
            PieceTypeIdentifier = pieceTypeIdentifier;
            PositionModel = positionModel;
        }

        public int PieceIdentifier { get; }

        public int PieceTypeIdentifier { get; }

        public GridPositionModel PositionModel { get; private set; }

        public void UpdatePositionModel(GridPositionModel positionModel)
        {
            PositionModel = positionModel;
        }
    }
}
