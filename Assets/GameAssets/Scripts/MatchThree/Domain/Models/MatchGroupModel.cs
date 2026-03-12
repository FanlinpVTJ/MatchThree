using System.Collections.Generic;
using MatchThree.Domain.ValueObjects;

namespace MatchThree.Domain.Models
{
    public class MatchGroupModel
    {
        public MatchGroupModel(int pieceTypeIdentifier, List<GridPositionModel> positionModels)
        {
            PieceTypeIdentifier = pieceTypeIdentifier;
            PositionModels = positionModels;
        }

        public int PieceTypeIdentifier { get; }

        public List<GridPositionModel> PositionModels { get; }
    }
}
