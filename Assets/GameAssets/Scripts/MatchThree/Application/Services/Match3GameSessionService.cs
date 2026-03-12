using System.Collections.Generic;
using MatchThree.Application.Interfaces;
using MatchThree.Domain.Enums;
using MatchThree.Domain.Interfaces;
using MatchThree.Domain.Models;
using R3;

namespace MatchThree.Application.Services
{
    public class Match3GameSessionService : IMatch3GameSessionService
    {
        private readonly IBoardInitializationService _boardInitializationService;
        private readonly ISwapValidationRule _swapValidationRule;
        private readonly IMatchDetectionRule _matchDetectionRule;
        private readonly ReactiveProperty<BoardModel> _boardModelProperty;
        private readonly ReactiveProperty<GamePhaseType> _gamePhaseTypeProperty;
        private readonly Subject<SwapValidationResultModel> _swapValidationResultModelSubject;

        public Match3GameSessionService(
            IBoardInitializationService boardInitializationService,
            ISwapValidationRule swapValidationRule,
            IMatchDetectionRule matchDetectionRule)
        {
            _boardInitializationService = boardInitializationService;
            _swapValidationRule = swapValidationRule;
            _matchDetectionRule = matchDetectionRule;
            _boardModelProperty = new ReactiveProperty<BoardModel>(null);
            _gamePhaseTypeProperty = new ReactiveProperty<GamePhaseType>(GamePhaseType.None);
            _swapValidationResultModelSubject = new Subject<SwapValidationResultModel>();
        }

        public Observable<BoardModel> ObserveBoardModel()
        {
            return _boardModelProperty;
        }

        public Observable<GamePhaseType> ObserveGamePhaseType()
        {
            return _gamePhaseTypeProperty;
        }

        public Observable<SwapValidationResultModel> ObserveSwapValidationResultModel()
        {
            return _swapValidationResultModelSubject;
        }

        public BoardModel GetCurrentBoardModel()
        {
            return _boardModelProperty.Value;
        }

        public GamePhaseType GetCurrentGamePhaseType()
        {
            return _gamePhaseTypeProperty.Value;
        }

        public void InitializeSession(int widthValue, int heightValue, int pieceTypeCountValue)
        {
            _gamePhaseTypeProperty.Value = GamePhaseType.Bootstrap;

            BoardModel boardModel = _boardInitializationService.CreateBoardModel(widthValue, heightValue, pieceTypeCountValue);

            _boardModelProperty.Value = boardModel;
            _gamePhaseTypeProperty.Value = GamePhaseType.Input;
        }

        public void TrySwap(SwapCommandModel swapCommandModel)
        {
            BoardModel boardModel = _boardModelProperty.Value;
            SwapValidationResultModel swapValidationResultModel = _swapValidationRule.Validate(boardModel, swapCommandModel);

            _swapValidationResultModelSubject.OnNext(swapValidationResultModel);

            if (swapValidationResultModel.IsValid == false)
            {
                return;
            }

            _gamePhaseTypeProperty.Value = GamePhaseType.Resolving;

            List<MatchGroupModel> matchGroupModels = _matchDetectionRule.FindMatchGroupModels(boardModel);

            if (matchGroupModels.Count == 0)
            {
                _gamePhaseTypeProperty.Value = GamePhaseType.Input;
                return;
            }

            _gamePhaseTypeProperty.Value = GamePhaseType.Input;
        }
    }
}
