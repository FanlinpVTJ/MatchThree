using MatchThree.Domain.Enums;
using MatchThree.Domain.Models;
using R3;

namespace MatchThree.Application.Interfaces
{
    public interface IMatch3GameSessionService
    {
        Observable<BoardModel> ObserveBoardModel();

        Observable<GamePhaseType> ObserveGamePhaseType();

        Observable<SwapValidationResultModel> ObserveSwapValidationResultModel();

        BoardModel GetCurrentBoardModel();

        GamePhaseType GetCurrentGamePhaseType();

        void InitializeSession(int widthValue, int heightValue, int pieceTypeCountValue);

        void TrySwap(SwapCommandModel swapCommandModel);
    }
}
