using MatchThree.Domain.Models;

namespace MatchThree.Domain.Interfaces
{
    public interface ISwapValidationRule
    {
        SwapValidationResultModel Validate(BoardModel boardModel, SwapCommandModel swapCommandModel);
    }
}
