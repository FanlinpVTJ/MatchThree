using MatchThree.Domain.Models;
using MatchThree.Domain.ValueObjects;

namespace MatchThree.Domain.Interfaces
{
    public interface ICellModelFactory
    {
        CellModel CreateCellModel(GridPositionModel positionModel, PieceModel pieceModel);
    }
}
