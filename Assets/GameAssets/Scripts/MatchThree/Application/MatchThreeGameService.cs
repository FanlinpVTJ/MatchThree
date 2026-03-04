using MatchThree.Application.Events;
using MatchThree.Domain;
using MessagePipe;

namespace MatchThree.Application
{
    public class MatchThreeGameService
    {
        private readonly BoardInitializer _boardInitializer;

        private readonly MoveProcessor _moveProcessor;

        private readonly BoardReshuffler _boardReshuffler;

        private readonly MatchThreeGameSettingsModel _settingsModel;

        private readonly IPublisher<GameProgressChangedEventModel> _gameProgressChangedEventPublisher;

        private readonly IPublisher<GameFinishedEventModel> _gameFinishedEventPublisher;

        private readonly IPublisher<BoardReshuffledEventModel> _boardReshuffledEventPublisher;

        private BoardModel _boardModel;

        private int _movesUsed;

        private int _totalMatchGroupCount;

        private bool _isFinished;

        public MatchThreeGameService(
            BoardInitializer boardInitializer,
            MoveProcessor moveProcessor,
            BoardReshuffler boardReshuffler,
            MatchThreeGameSettingsModel settingsModel,
            IPublisher<GameProgressChangedEventModel> gameProgressChangedEventPublisher,
            IPublisher<GameFinishedEventModel> gameFinishedEventPublisher,
            IPublisher<BoardReshuffledEventModel> boardReshuffledEventPublisher)
        {
            _boardInitializer = boardInitializer;
            _moveProcessor = moveProcessor;
            _boardReshuffler = boardReshuffler;
            _settingsModel = settingsModel;
            _gameProgressChangedEventPublisher = gameProgressChangedEventPublisher;
            _gameFinishedEventPublisher = gameFinishedEventPublisher;
            _boardReshuffledEventPublisher = boardReshuffledEventPublisher;
        }

        public void StartNewGame()
        {
            _boardModel = _boardInitializer.CreateBoard(_settingsModel.BoardWidth, _settingsModel.BoardHeight);
            _movesUsed = 0;
            _totalMatchGroupCount = 0;
            _isFinished = false;
            EnsureBoardPlayable();

            PublishProgressChangedEvent();
        }

        public void TryProcessConfiguredStartMove()
        {
            if (!_settingsModel.RunMoveOnStart)
            {
                return;
            }

            BoardCoordinate fromCoordinate = new BoardCoordinate(_settingsModel.FromColumn, _settingsModel.FromRow);
            BoardCoordinate toCoordinate = new BoardCoordinate(_settingsModel.ToColumn, _settingsModel.ToRow);
            MoveModel moveModel = new MoveModel(fromCoordinate, toCoordinate);
            TryProcessMove(moveModel);
        }

        public MoveProcessResultModel TryProcessMove(MoveModel moveModel)
        {
            if (_isFinished)
            {
                MoveProcessResultModel finishedResult = MoveProcessResultModel.Failed("Game is already finished.");

                return finishedResult;
            }

            MoveProcessResultModel resultModel = _moveProcessor.Process(_boardModel, moveModel);

            if (!resultModel.IsApplied)
            {
                return resultModel;
            }

            _movesUsed++;
            _totalMatchGroupCount += resultModel.MatchGroups.Count;
            PublishProgressChangedEvent();

            if (_totalMatchGroupCount >= _settingsModel.TargetMatchGroupCount)
            {
                _isFinished = true;
                PublishFinishedEvent(true);
            }
            else if (_movesUsed >= _settingsModel.MoveLimit)
            {
                _isFinished = true;
                PublishFinishedEvent(false);
            }
            else
            {
                EnsureBoardPlayable();
            }

            return resultModel;
        }

        private void EnsureBoardPlayable()
        {
            int reshuffleAttemptCount = _boardReshuffler.EnsurePlayable(_boardModel);

            if (reshuffleAttemptCount <= 0)
            {
                return;
            }

            BoardReshuffledEventModel eventModel = new BoardReshuffledEventModel(reshuffleAttemptCount);
            _boardReshuffledEventPublisher.Publish(eventModel);
        }

        private void PublishProgressChangedEvent()
        {
            int movesRemaining = _settingsModel.MoveLimit - _movesUsed;

            if (movesRemaining < 0)
            {
                movesRemaining = 0;
            }

            GameProgressChangedEventModel eventModel = new GameProgressChangedEventModel(
                _movesUsed,
                movesRemaining,
                _totalMatchGroupCount,
                _settingsModel.TargetMatchGroupCount);
            _gameProgressChangedEventPublisher.Publish(eventModel);
        }

        private void PublishFinishedEvent(bool isWin)
        {
            GameFinishedEventModel eventModel = new GameFinishedEventModel(
                isWin,
                _movesUsed,
                _totalMatchGroupCount,
                _settingsModel.TargetMatchGroupCount);
            _gameFinishedEventPublisher.Publish(eventModel);
        }
    }
}
