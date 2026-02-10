using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MatchThree.Application;
using MatchThree.Application.Events;
using MatchThree.Domain;
using MessagePipe;
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

        private ISubscriber<MoveStartedEventModel> _moveStartedEventSubscriber;

        private ISubscriber<MoveRejectedEventModel> _moveRejectedEventSubscriber;

        private ISubscriber<MoveAppliedEventModel> _moveAppliedEventSubscriber;

        private ISubscriber<CascadeResolvedEventModel> _cascadeResolvedEventSubscriber;

        private ISubscriber<BoardSettledEventModel> _boardSettledEventSubscriber;

        private List<IDisposable> _subscriptions;

        [Inject]
        private void Construct(
            BoardInitializer boardInitializer,
            MoveProcessor moveProcessor,
            ISubscriber<MoveStartedEventModel> moveStartedEventSubscriber,
            ISubscriber<MoveRejectedEventModel> moveRejectedEventSubscriber,
            ISubscriber<MoveAppliedEventModel> moveAppliedEventSubscriber,
            ISubscriber<CascadeResolvedEventModel> cascadeResolvedEventSubscriber,
            ISubscriber<BoardSettledEventModel> boardSettledEventSubscriber)
        {
            _boardInitializer = boardInitializer;
            _moveProcessor = moveProcessor;
            _moveStartedEventSubscriber = moveStartedEventSubscriber;
            _moveRejectedEventSubscriber = moveRejectedEventSubscriber;
            _moveAppliedEventSubscriber = moveAppliedEventSubscriber;
            _cascadeResolvedEventSubscriber = cascadeResolvedEventSubscriber;
            _boardSettledEventSubscriber = boardSettledEventSubscriber;
            _subscriptions = new List<IDisposable>();
        }

        private void Start()
        {
            SubscribeToEvents();
            _boardModel = _boardInitializer.CreateBoard(_boardWidth, _boardHeight);

            if (!_runMoveOnStart)
            {
                Debug.Log("MatchThree board is initialized.");

                return;
            }

            BoardCoordinate fromCoordinate = new BoardCoordinate(_fromColumn, _fromRow);
            BoardCoordinate toCoordinate = new BoardCoordinate(_toColumn, _toRow);
            MoveModel moveModel = new MoveModel(fromCoordinate, toCoordinate);
            _moveProcessor.Process(_boardModel, moveModel);
        }

        private void OnDestroy()
        {
            for (int index = 0; index < _subscriptions.Count; index++)
            {
                IDisposable subscription = _subscriptions[index];
                subscription.Dispose();
            }
        }

        private void SubscribeToEvents()
        {
            IDisposable moveStartedSubscription = _moveStartedEventSubscriber.Subscribe(HandleMoveStarted);
            _subscriptions.Add(moveStartedSubscription);

            IDisposable moveRejectedSubscription = _moveRejectedEventSubscriber.Subscribe(HandleMoveRejected);
            _subscriptions.Add(moveRejectedSubscription);

            IDisposable moveAppliedSubscription = _moveAppliedEventSubscriber.Subscribe(HandleMoveApplied);
            _subscriptions.Add(moveAppliedSubscription);

            IDisposable cascadeResolvedSubscription = _cascadeResolvedEventSubscriber.Subscribe(HandleCascadeResolved);
            _subscriptions.Add(cascadeResolvedSubscription);

            IDisposable boardSettledSubscription = _boardSettledEventSubscriber.Subscribe(HandleBoardSettled);
            _subscriptions.Add(boardSettledSubscription);
        }

        private static void HandleMoveStarted(MoveStartedEventModel eventModel)
        {
            BoardCoordinate fromCoordinate = eventModel.MoveModel.FromCoordinate;
            BoardCoordinate toCoordinate = eventModel.MoveModel.ToCoordinate;
            Debug.Log($"Move started: ({fromCoordinate.Column}, {fromCoordinate.Row}) -> ({toCoordinate.Column}, {toCoordinate.Row})");
        }

        private static void HandleMoveRejected(MoveRejectedEventModel eventModel)
        {
            Debug.LogWarning($"Move rejected: {eventModel.ErrorMessage}");
        }

        private static void HandleMoveApplied(MoveAppliedEventModel eventModel)
        {
            Debug.Log($"Move applied. Groups: {eventModel.TotalMatchGroupCount}, Cascades: {eventModel.CascadeCount}");
        }

        private static void HandleCascadeResolved(CascadeResolvedEventModel eventModel)
        {
            Debug.Log($"Cascade resolved. Index: {eventModel.CascadeIndex}, Groups: {eventModel.MatchGroupCount}");
        }

        private static void HandleBoardSettled(BoardSettledEventModel eventModel)
        {
            Debug.Log($"Board settled. Total groups: {eventModel.TotalMatchGroupCount}, Cascades: {eventModel.CascadeCount}");
        }
    }
}
