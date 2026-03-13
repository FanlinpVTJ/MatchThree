using System;
using MatchThree.Application.Interfaces;
using MatchThree.Domain.Enums;
using MatchThree.Domain.Models;
using MatchThree.Domain.ValueObjects;
using R3;

namespace MatchThree.Presentation.ViewModels
{
    public class GameSessionViewModel : ViewModelBase
    {
        private readonly ReactiveProperty<GamePhaseType> _gamePhaseTypeProperty;
        private readonly ReactiveProperty<GridPositionModel> _selectedPositionModelProperty;
        private readonly ReactiveProperty<string> _swapValidationMessageProperty;

        public GameSessionViewModel(IMatch3GameSessionService match3GameSessionService, BoardViewModel boardViewModel)
        {
            Match3GameSessionService = match3GameSessionService;
            BoardViewModel = boardViewModel;
            _gamePhaseTypeProperty = new ReactiveProperty<GamePhaseType>(match3GameSessionService.GetCurrentGamePhaseType());
            _selectedPositionModelProperty = new ReactiveProperty<GridPositionModel>(CreateEmptySelectedPositionModel());
            _swapValidationMessageProperty = new ReactiveProperty<string>(string.Empty);

            IDisposable gamePhaseDisposable = match3GameSessionService.ObserveGamePhaseType()
                .Subscribe(this, static (gamePhaseType, gameSessionViewModel) => gameSessionViewModel.ApplyGamePhaseType(gamePhaseType));

            IDisposable swapValidationDisposable = match3GameSessionService.ObserveSwapValidationResultModel()
                .Subscribe(this, static (swapValidationResultModel, gameSessionViewModel) => gameSessionViewModel.ApplySwapValidationResultModel(swapValidationResultModel));

            AddDisposable(gamePhaseDisposable);
            AddDisposable(swapValidationDisposable);
        }

        public IMatch3GameSessionService Match3GameSessionService { get; }

        public BoardViewModel BoardViewModel { get; }

        public Observable<GamePhaseType> ObserveGamePhaseType()
        {
            return _gamePhaseTypeProperty;
        }

        public Observable<GridPositionModel> ObserveSelectedPositionModel()
        {
            return _selectedPositionModelProperty;
        }

        public Observable<string> ObserveSwapValidationMessage()
        {
            return _swapValidationMessageProperty;
        }

        public GamePhaseType GetCurrentGamePhaseType()
        {
            return _gamePhaseTypeProperty.Value;
        }

        public void InitializeSession(int widthValue, int heightValue, int pieceTypeCountValue)
        {
            Match3GameSessionService.InitializeSession(widthValue, heightValue, pieceTypeCountValue);
        }

        public void TrySelectCell(GridPositionModel positionModel)
        {
            if (_gamePhaseTypeProperty.Value != GamePhaseType.Input)
            {
                return;
            }

            if (HasSelectedPosition() == false)
            {
                _selectedPositionModelProperty.Value = positionModel;
                return;
            }

            if (_selectedPositionModelProperty.Value == positionModel)
            {
                ResetSelectedPositionModel();
                return;
            }

            SwapCommandModel swapCommandModel = new SwapCommandModel(_selectedPositionModelProperty.Value, positionModel);
            ResetSelectedPositionModel();
            Match3GameSessionService.TrySwap(swapCommandModel);
        }

        private void ApplyGamePhaseType(GamePhaseType gamePhaseType)
        {
            _gamePhaseTypeProperty.Value = gamePhaseType;
        }

        private void ApplySwapValidationResultModel(SwapValidationResultModel swapValidationResultModel)
        {
            _swapValidationMessageProperty.Value = swapValidationResultModel.ValidationMessage;
        }

        private bool HasSelectedPosition()
        {
            return _selectedPositionModelProperty.Value.RowIndex >= 0 && _selectedPositionModelProperty.Value.ColumnIndex >= 0;
        }

        private void ResetSelectedPositionModel()
        {
            _selectedPositionModelProperty.Value = CreateEmptySelectedPositionModel();
        }

        private GridPositionModel CreateEmptySelectedPositionModel()
        {
            GridPositionModel positionModel = new GridPositionModel(-1, -1);

            return positionModel;
        }
    }
}
