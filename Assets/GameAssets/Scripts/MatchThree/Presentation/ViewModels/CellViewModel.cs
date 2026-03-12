using MatchThree.Domain.Models;
using R3;

namespace MatchThree.Presentation.ViewModels
{
    public class CellViewModel : ViewModelBase
    {
        private readonly ReactiveProperty<CellModel> _cellModelProperty;

        public CellViewModel(CellModel cellModel)
        {
            _cellModelProperty = new ReactiveProperty<CellModel>(cellModel);
        }

        public Observable<CellModel> ObserveCellModel()
        {
            return _cellModelProperty;
        }

        public CellModel GetCurrentCellModel()
        {
            return _cellModelProperty.Value;
        }

        public void ApplyCellModel(CellModel cellModel)
        {
            _cellModelProperty.Value = cellModel;
        }
    }
}
