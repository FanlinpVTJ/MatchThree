using System;
using MatchThree.Application.Interfaces;
using MatchThree.Domain.Models;
using R3;

namespace MatchThree.Presentation.ViewModels
{
    public class BoardViewModel : ViewModelBase
    {
        private readonly ReactiveProperty<BoardModel> _boardModelProperty;

        public BoardViewModel(IMatch3GameSessionService match3GameSessionService)
        {
            _boardModelProperty = new ReactiveProperty<BoardModel>(match3GameSessionService.GetCurrentBoardModel());

            IDisposable boardModelDisposable = match3GameSessionService.ObserveBoardModel()
                .Subscribe(this, static (boardModel, boardViewModel) => boardViewModel.ApplyBoardModel(boardModel));

            AddDisposable(boardModelDisposable);
        }

        public Observable<BoardModel> ObserveBoardModel()
        {
            return _boardModelProperty;
        }

        public BoardModel GetCurrentBoardModel()
        {
            return _boardModelProperty.Value;
        }

        private void ApplyBoardModel(BoardModel boardModel)
        {
            _boardModelProperty.Value = boardModel;
        }
    }
}
