using System;
using System.Collections.Generic;
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
        private BoardInitializer _boardInitializer;

        private MoveProcessor _moveProcessor;

        private MatchThreeGameSettingsModel _settingsModel;

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
            MatchThreeGameSettingsModel settingsModel,
            ISubscriber<MoveStartedEventModel> moveStartedEventSubscriber,
            ISubscriber<MoveRejectedEventModel> moveRejectedEventSubscriber,
            ISubscriber<MoveAppliedEventModel> moveAppliedEventSubscriber,
            ISubscriber<CascadeResolvedEventModel> cascadeResolvedEventSubscriber,
            ISubscriber<BoardSettledEventModel> boardSettledEventSubscriber)
        {
            _boardInitializer = boardInitializer;
            _moveProcessor = moveProcessor;
            _settingsModel = settingsModel;
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
            _boardModel = _boardInitializer.CreateBoard(_settingsModel.BoardWidth, _settingsModel.BoardHeight);

            if (!_settingsModel.RunMoveOnStart)
            {
                Debug.Log("MatchThree board is initialized.");

                return;
            }

            BoardCoordinate fromCoordinate = new BoardCoordinate(_settingsModel.FromColumn, _settingsModel.FromRow);
            BoardCoordinate toCoordinate = new BoardCoordinate(_settingsModel.ToColumn, _settingsModel.ToRow);
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
