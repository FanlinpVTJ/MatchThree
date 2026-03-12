using MatchThree.Domain.Models;
using MatchThree.Domain.ValueObjects;

namespace MatchThree.Domain.Interfaces
{
    public interface IPieceModelFactory
    {
        PieceModel CreatePieceModel(int pieceIdentifier, int pieceTypeIdentifier, GridPositionModel positionModel);
    }
}
