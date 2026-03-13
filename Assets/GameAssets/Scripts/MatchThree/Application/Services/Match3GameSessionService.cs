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
        private readonly IBoardSwapService _boardSwapService;
        private readonly IMatchResolutionService _matchResolutionService;
        private readonly ISwapValidationRule _swapValidationRule;
        private readonly IMatchDetectionRule _matchDetectionRule;
        private readonly Subject<BoardModel> _boardModelSubject;
        private readonly ReactiveProperty<GamePhaseType> _gamePhaseTypeProperty;
        private readonly Subject<SwapValidationResultModel> _swapValidationResultModelSubject;
        private BoardModel _currentBoardModel;

        public Match3GameSessionService(
            IBoardInitializationService boardInitializationService,
            IBoardSwapService boardSwapService,
            IMatchResolutionService matchResolutionService,
            ISwapValidationRule swapValidationRule,
            IMatchDetectionRule matchDetectionRule)
        {
            _boardInitializationService = boardInitializationService;
            _boardSwapService = boardSwapService;
            _matchResolutionService = matchResolutionService;
            _swapValidationRule = swapValidationRule;
            _matchDetectionRule = matchDetectionRule;
            _boardModelSubject = new Subject<BoardModel>();
            _gamePhaseTypeProperty = new ReactiveProperty<GamePhaseType>(GamePhaseType.None);
            _swapValidationResultModelSubject = new Subject<SwapValidationResultModel>();
        }

        public Observable<BoardModel> ObserveBoardModel()
        {
            return _boardModelSubject;
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
            return _currentBoardModel;
        }

        public GamePhaseType GetCurrentGamePhaseType()
        {
            return _gamePhaseTypeProperty.Value;
        }

        public void InitializeSession(int widthValue, int heightValue, int pieceTypeCountValue)
        {
            _gamePhaseTypeProperty.Value = GamePhaseType.Bootstrap;

            BoardModel boardModel = _boardInitializationService.CreateBoardModel(widthValue, heightValue, pieceTypeCountValue);

            _currentBoardModel = boardModel;
            _boardModelSubject.OnNext(_currentBoardModel);
            _gamePhaseTypeProperty.Value = GamePhaseType.Input;
        }

        public void TrySwap(SwapCommandModel swapCommandModel)
        {
            BoardModel boardModel = _currentBoardModel;
            SwapValidationResultModel swapValidationResultModel = _swapValidationRule.Validate(boardModel, swapCommandModel);

            _swapValidationResultModelSubject.OnNext(swapValidationResultModel);

            if (swapValidationResultModel.IsValid == false)
            {
                return;
            }

            _gamePhaseTypeProperty.Value = GamePhaseType.Resolving;

            BoardModel swappedBoardModel = _boardSwapService.CreateSwappedBoardModel(boardModel, swapCommandModel);
            List<MatchGroupModel> matchGroupModels = _matchDetectionRule.FindMatchGroupModels(swappedBoardModel);

            if (matchGroupModels.Count == 0)
            {
                SwapValidationResultModel failedSwapValidationResultModel = new SwapValidationResultModel(false, "Swap should produce a match.");
                _swapValidationResultModelSubject.OnNext(failedSwapValidationResultModel);
                _gamePhaseTypeProperty.Value = GamePhaseType.Input;
                return;
            }

            BoardModel resolvedBoardModel = _matchResolutionService.CreateResolvedBoardModel(swappedBoardModel, matchGroupModels);
            _currentBoardModel = resolvedBoardModel;
            _boardModelSubject.OnNext(_currentBoardModel);
            _gamePhaseTypeProperty.Value = GamePhaseType.Input;
        }
    }
}
