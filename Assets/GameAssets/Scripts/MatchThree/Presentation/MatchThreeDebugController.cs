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
        private MatchThreeGameService _gameService;

        private ISubscriber<MoveStartedEventModel> _moveStartedEventSubscriber;

        private ISubscriber<MoveRejectedEventModel> _moveRejectedEventSubscriber;

        private ISubscriber<MoveAppliedEventModel> _moveAppliedEventSubscriber;

        private ISubscriber<CascadeResolvedEventModel> _cascadeResolvedEventSubscriber;

        private ISubscriber<BoardSettledEventModel> _boardSettledEventSubscriber;

        private ISubscriber<GameProgressChangedEventModel> _gameProgressChangedEventSubscriber;

        private ISubscriber<GameFinishedEventModel> _gameFinishedEventSubscriber;

        private List<IDisposable> _subscriptions;

        [Inject]
        private void Construct(
            MatchThreeGameService gameService,
            ISubscriber<MoveStartedEventModel> moveStartedEventSubscriber,
            ISubscriber<MoveRejectedEventModel> moveRejectedEventSubscriber,
            ISubscriber<MoveAppliedEventModel> moveAppliedEventSubscriber,
            ISubscriber<CascadeResolvedEventModel> cascadeResolvedEventSubscriber,
            ISubscriber<BoardSettledEventModel> boardSettledEventSubscriber,
            ISubscriber<GameProgressChangedEventModel> gameProgressChangedEventSubscriber,
            ISubscriber<GameFinishedEventModel> gameFinishedEventSubscriber)
        {
            _gameService = gameService;
            _moveStartedEventSubscriber = moveStartedEventSubscriber;
            _moveRejectedEventSubscriber = moveRejectedEventSubscriber;
            _moveAppliedEventSubscriber = moveAppliedEventSubscriber;
            _cascadeResolvedEventSubscriber = cascadeResolvedEventSubscriber;
            _boardSettledEventSubscriber = boardSettledEventSubscriber;
            _gameProgressChangedEventSubscriber = gameProgressChangedEventSubscriber;
            _gameFinishedEventSubscriber = gameFinishedEventSubscriber;
            _subscriptions = new List<IDisposable>();
        }

        private void Start()
        {
            SubscribeToEvents();
            _gameService.StartNewGame();
            _gameService.TryProcessConfiguredStartMove();
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

            IDisposable gameProgressChangedSubscription = _gameProgressChangedEventSubscriber.Subscribe(HandleGameProgressChanged);
            _subscriptions.Add(gameProgressChangedSubscription);

            IDisposable gameFinishedSubscription = _gameFinishedEventSubscriber.Subscribe(HandleGameFinished);
            _subscriptions.Add(gameFinishedSubscription);
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

        private static void HandleGameProgressChanged(GameProgressChangedEventModel eventModel)
        {
            Debug.Log($"Progress changed. Moves used: {eventModel.MovesUsed}, Moves remaining: {eventModel.MovesRemaining}, Goal: {eventModel.TotalMatchGroupCount}/{eventModel.TargetMatchGroupCount}");
        }

        private static void HandleGameFinished(GameFinishedEventModel eventModel)
        {
            string result = eventModel.IsWin ? "Win" : "Lose";
            Debug.Log($"Game finished. Result: {result}, Moves used: {eventModel.MovesUsed}, Goal: {eventModel.TotalMatchGroupCount}/{eventModel.TargetMatchGroupCount}");
        }
    }
}
