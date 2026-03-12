using MatchThree.Domain.Models;

namespace MatchThree.Domain.Interfaces
{
    public interface IBoardInitializationService
    {
        BoardModel CreateBoardModel(int widthValue, int heightValue, int pieceTypeCountValue);
    }
}
