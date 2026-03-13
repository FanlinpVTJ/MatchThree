using MatchThree.Presentation.Interfaces;
using MatchThree.Presentation.ViewModels;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThree.Presentation.Views
{
    public class CellView : MonoBehaviour, IView<CellViewModel>
    {
        private Button _button;
        private Image _backgroundImage;
        private TextMeshProUGUI _labelText;
        private CellViewModel _cellViewModel;

        public void Initialize(Button button, Image backgroundImage, TextMeshProUGUI labelText)
        {
            _button = button;
            _backgroundImage = backgroundImage;
            _labelText = labelText;
        }

        public void Bind(CellViewModel viewModel)
        {
            _cellViewModel = viewModel;

            viewModel.ObserveCellModel()
                .Subscribe(this, static (cellModel, cellView) => cellView.ApplyCellModel(cellModel))
                .AddTo(this);

            viewModel.ObserveIsSelected()
                .Subscribe(this, static (isSelected, cellView) => cellView.ApplySelectedState(isSelected))
                .AddTo(this);

            _button.OnClickAsObservable()
                .Subscribe(this, static (_, cellView) => cellView._cellViewModel.SelectCell())
                .AddTo(this);
        }

        private void OnDestroy()
        {
            if (_cellViewModel != null)
            {
                _cellViewModel.Dispose();
            }
        }

        private void ApplyCellModel(Domain.Models.CellModel cellModel)
        {
            if (cellModel == null || cellModel.PieceModel == null)
            {
                _labelText.text = string.Empty;
                _backgroundImage.color = new Color(0.18f, 0.18f, 0.2f, 1f);
                return;
            }

            _labelText.text = cellModel.PieceModel.PieceTypeIdentifier.ToString();
            _backgroundImage.color = CreatePieceColor(cellModel.PieceModel.PieceTypeIdentifier);
        }

        private void ApplySelectedState(bool isSelected)
        {
            Color backgroundColor = _backgroundImage.color;
            backgroundColor.a = isSelected ? 0.75f : 1f;
            _backgroundImage.color = backgroundColor;
        }

        private Color CreatePieceColor(int pieceTypeIdentifier)
        {
            switch (pieceTypeIdentifier)
            {
                case 1:
                    return new Color(0.85f, 0.33f, 0.31f, 1f);
                case 2:
                    return new Color(0.31f, 0.59f, 0.89f, 1f);
                case 3:
                    return new Color(0.35f, 0.76f, 0.39f, 1f);
                case 4:
                    return new Color(0.95f, 0.77f, 0.24f, 1f);
                case 5:
                    return new Color(0.64f, 0.4f, 0.82f, 1f);
                case 6:
                    return new Color(0.96f, 0.54f, 0.19f, 1f);
            }

            return new Color(0.42f, 0.42f, 0.42f, 1f);
        }
    }
}
