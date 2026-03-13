using MatchThree.Infrastructure.Factories;
using MatchThree.Presentation.ViewModels;
using R3;
using UnityEngine;
using Zenject;

namespace MatchThree.Presentation.Views
{
    public class MatchThreeSceneEntryPoint : MonoBehaviour
    {
        [SerializeField]
        private BoardView _boardView;

        private GameSessionViewModel _gameSessionViewModel;
        private CellViewFactory _cellViewFactory;
        private CellViewModelFactory _cellViewModelFactory;
        private int _boardWidthValue;
        private int _boardHeightValue;
        private int _pieceTypeCountValue;

        [Inject]
        public void Construct(
            GameSessionViewModel gameSessionViewModel,
            CellViewFactory cellViewFactory,
            CellViewModelFactory cellViewModelFactory,
            [Inject(Id = "BoardWidthValue")] int boardWidthValue,
            [Inject(Id = "BoardHeightValue")] int boardHeightValue,
            [Inject(Id = "PieceTypeCountValue")] int pieceTypeCountValue)
        {
            _gameSessionViewModel = gameSessionViewModel;
            _cellViewFactory = cellViewFactory;
            _cellViewModelFactory = cellViewModelFactory;
            _boardWidthValue = boardWidthValue;
            _boardHeightValue = boardHeightValue;
            _pieceTypeCountValue = pieceTypeCountValue;
        }

        private void Start()
        {
            _boardView.Initialize(_gameSessionViewModel, _cellViewFactory, _cellViewModelFactory);

            _gameSessionViewModel.ObserveSwapValidationMessage()
                .Subscribe(this, static (validationMessage, entryPoint) => entryPoint.ApplyValidationMessage(validationMessage))
                .AddTo(this);

            _gameSessionViewModel.InitializeSession(_boardWidthValue, _boardHeightValue, _pieceTypeCountValue);
        }

        private void ApplyValidationMessage(string validationMessage)
        {
            if (string.IsNullOrWhiteSpace(validationMessage))
            {
                return;
            }

            Debug.LogWarning(validationMessage);
        }
    }
}
