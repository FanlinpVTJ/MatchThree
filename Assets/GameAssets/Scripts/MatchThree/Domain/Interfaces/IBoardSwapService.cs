using MatchThree.Domain.Models;

namespace MatchThree.Domain.Interfaces
{
    public interface IBoardSwapService
    {
        BoardModel CreateSwappedBoardModel(BoardModel boardModel, SwapCommandModel swapCommandModel);
    }
}
