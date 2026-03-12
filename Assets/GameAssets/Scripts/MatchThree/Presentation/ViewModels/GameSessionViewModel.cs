using System;
using MatchThree.Application.Interfaces;
using MatchThree.Domain.Enums;
using R3;

namespace MatchThree.Presentation.ViewModels
{
    public class GameSessionViewModel : ViewModelBase
    {
        private readonly ReactiveProperty<GamePhaseType> _gamePhaseTypeProperty;

        public GameSessionViewModel(IMatch3GameSessionService match3GameSessionService, BoardViewModel boardViewModel)
        {
            Match3GameSessionService = match3GameSessionService;
            BoardViewModel = boardViewModel;
            _gamePhaseTypeProperty = new ReactiveProperty<GamePhaseType>(match3GameSessionService.GetCurrentGamePhaseType());

            IDisposable gamePhaseDisposable = match3GameSessionService.ObserveGamePhaseType()
                .Subscribe(this, static (gamePhaseType, gameSessionViewModel) => gameSessionViewModel.ApplyGamePhaseType(gamePhaseType));

            AddDisposable(gamePhaseDisposable);
        }

        public IMatch3GameSessionService Match3GameSessionService { get; }

        public BoardViewModel BoardViewModel { get; }

        public Observable<GamePhaseType> ObserveGamePhaseType()
        {
            return _gamePhaseTypeProperty;
        }

        public GamePhaseType GetCurrentGamePhaseType()
        {
            return _gamePhaseTypeProperty.Value;
        }

        private void ApplyGamePhaseType(GamePhaseType gamePhaseType)
        {
            _gamePhaseTypeProperty.Value = gamePhaseType;
        }
    }
}
