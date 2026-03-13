using System.Collections.Generic;
using MatchThree.Domain.Models;
using MatchThree.Presentation.Interfaces;
using MatchThree.Presentation.ViewModels;
using MatchThree.Infrastructure.Factories;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace MatchThree.Presentation.Views
{
    [RequireComponent(typeof(RectTransform))]
    public class BoardView : MonoBehaviour, IView<GameSessionViewModel>
    {
        [SerializeField]
        private Vector2 _cellSizeValue = new Vector2(72f, 72f);

        [SerializeField]
        private Vector2 _spacingValue = new Vector2(8f, 8f);

        private readonly List<CellView> _cellViews = new List<CellView>();
        private CellViewFactory _cellViewFactory;
        private CellViewModelFactory _cellViewModelFactory;
        private GridLayoutGroup _gridLayoutGroup;
        private bool _isBound;

        public void Initialize(
            GameSessionViewModel gameSessionViewModel,
            CellViewFactory cellViewFactory,
            CellViewModelFactory cellViewModelFactory)
        {
            if (_isBound)
            {
                return;
            }

            _isBound = true;
            _cellViewFactory = cellViewFactory;
            _cellViewModelFactory = cellViewModelFactory;
            EnsureGridLayoutGroup();
            Bind(gameSessionViewModel);
        }

        public void Bind(GameSessionViewModel viewModel)
        {
            viewModel.BoardViewModel.ObserveBoardModel()
                .Subscribe(this, static (boardModel, boardView) => boardView.RebuildBoard(boardModel))
                .AddTo(this);
        }

        private void EnsureGridLayoutGroup()
        {
            _gridLayoutGroup = GetComponent<GridLayoutGroup>();

            if (_gridLayoutGroup == null)
            {
                _gridLayoutGroup = gameObject.AddComponent<GridLayoutGroup>();
            }

            _gridLayoutGroup.cellSize = _cellSizeValue;
            _gridLayoutGroup.spacing = _spacingValue;
            _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _gridLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
        }

        private void RebuildBoard(BoardModel boardModel)
        {
            ClearBoard();
            _gridLayoutGroup.constraintCount = boardModel.WidthValue;

            for (int cellIndex = 0; cellIndex < boardModel.CellModels.Count; cellIndex++)
            {
                CellModel cellModel = boardModel.CellModels[cellIndex];
                CellView cellView = _cellViewFactory.CreateCellView(transform);
                CellViewModel cellViewModel = _cellViewModelFactory.CreateCellViewModel(cellModel);
                cellView.Bind(cellViewModel);
                _cellViews.Add(cellView);
            }
        }

        private void ClearBoard()
        {
            for (int cellIndex = 0; cellIndex < _cellViews.Count; cellIndex++)
            {
                CellView cellView = _cellViews[cellIndex];

                if (cellView != null)
                {
                    Destroy(cellView.gameObject);
                }
            }

            _cellViews.Clear();
        }
    }
}
