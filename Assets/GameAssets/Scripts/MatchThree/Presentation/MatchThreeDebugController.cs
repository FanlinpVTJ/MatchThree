using MatchThree.Application;
using MatchThree.Domain;
using UnityEngine;
using Zenject;

namespace MatchThree.Presentation
{
    public class MatchThreeDebugController : MonoBehaviour
    {
        [SerializeField]
        private int _boardWidth = 8;

        [SerializeField]
        private int _boardHeight = 8;

        [SerializeField]
        private bool _runMoveOnStart = true;

        [SerializeField]
        private int _fromColumn;

        [SerializeField]
        private int _fromRow;

        [SerializeField]
        private int _toColumn = 1;

        [SerializeField]
        private int _toRow;

        private BoardInitializer _boardInitializer;

        private MoveProcessor _moveProcessor;

        private BoardModel _boardModel;

        [Inject]
        private void Construct(BoardInitializer boardInitializer, MoveProcessor moveProcessor)
        {
            _boardInitializer = boardInitializer;
            _moveProcessor = moveProcessor;
        }

        private void Start()
        {
            _boardModel = _boardInitializer.CreateBoard(_boardWidth, _boardHeight);

            if (!_runMoveOnStart)
            {
                Debug.Log("MatchThree board is initialized.");

                return;
            }

            BoardCoordinate fromCoordinate = new BoardCoordinate(_fromColumn, _fromRow);
            BoardCoordinate toCoordinate = new BoardCoordinate(_toColumn, _toRow);
            MoveModel moveModel = new MoveModel(fromCoordinate, toCoordinate);
            MoveProcessResultModel resultModel = _moveProcessor.Process(_boardModel, moveModel);

            if (!resultModel.IsApplied)
            {
                Debug.LogWarning(resultModel.ErrorMessage);

                return;
            }

            Debug.Log($"Move applied. Match groups count: {resultModel.MatchGroups.Count}");
        }
    }
}
