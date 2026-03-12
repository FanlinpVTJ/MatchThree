using MatchThree.Domain.Models;
using R3;

namespace MatchThree.Presentation.ViewModels
{
    public class PieceViewModel : ViewModelBase
    {
        private readonly ReactiveProperty<PieceModel> _pieceModelProperty;

        public PieceViewModel(PieceModel pieceModel)
        {
            _pieceModelProperty = new ReactiveProperty<PieceModel>(pieceModel);
        }

        public Observable<PieceModel> ObservePieceModel()
        {
            return _pieceModelProperty;
        }

        public PieceModel GetCurrentPieceModel()
        {
            return _pieceModelProperty.Value;
        }

        public void ApplyPieceModel(PieceModel pieceModel)
        {
            _pieceModelProperty.Value = pieceModel;
        }
    }
}
