using System;
using MatchThree.Domain.Models;
using MatchThree.Domain.ValueObjects;
using R3;

namespace MatchThree.Presentation.ViewModels
{
    public class CellViewModel : ViewModelBase
    {
        private readonly ReactiveProperty<CellModel> _cellModelProperty;
        private readonly ReactiveProperty<bool> _isSelectedProperty;
        private readonly GameSessionViewModel _gameSessionViewModel;

        public CellViewModel(CellModel cellModel, GameSessionViewModel gameSessionViewModel)
        {
            _gameSessionViewModel = gameSessionViewModel;
            _cellModelProperty = new ReactiveProperty<CellModel>(cellModel);
            _isSelectedProperty = new ReactiveProperty<bool>(false);

            IDisposable selectedPositionDisposable = gameSessionViewModel.ObserveSelectedPositionModel()
                .Subscribe(this, static (selectedPositionModel, cellViewModel) => cellViewModel.ApplySelectedPositionModel(selectedPositionModel));

            AddDisposable(selectedPositionDisposable);
        }

        public Observable<CellModel> ObserveCellModel()
        {
            return _cellModelProperty;
        }

        public Observable<bool> ObserveIsSelected()
        {
            return _isSelectedProperty;
        }

        public CellModel GetCurrentCellModel()
        {
            return _cellModelProperty.Value;
        }

        public void SelectCell()
        {
            _gameSessionViewModel.TrySelectCell(_cellModelProperty.Value.PositionModel);
        }

        public void ApplyCellModel(CellModel cellModel)
        {
            _cellModelProperty.Value = cellModel;
        }

        private void ApplySelectedPositionModel(GridPositionModel selectedPositionModel)
        {
            bool isSelected = _cellModelProperty.Value.PositionModel == selectedPositionModel;
            _isSelectedProperty.Value = isSelected;
        }
    }
}
