using System;
using System.Collections.Generic;
using MatchThree.Domain;
using MatchThree.Domain.Contracts;

namespace MatchThree.Application
{
    public class BoardReshuffler
    {
        private const int MaxReshuffleAttempts = 128;

        private readonly IRandomProvider _randomProvider;

        private readonly IMatchRule _matchRule;

        private readonly IPossibleMoveFinder _possibleMoveFinder;

        public BoardReshuffler(IRandomProvider randomProvider, IMatchRule matchRule, IPossibleMoveFinder possibleMoveFinder)
        {
            _randomProvider = randomProvider;
            _matchRule = matchRule;
            _possibleMoveFinder = possibleMoveFinder;
        }

        public int EnsurePlayable(BoardModel boardModel)
        {
            bool isPlayable = IsPlayable(boardModel);

            if (isPlayable)
            {
                return 0;
            }

            for (int attempt = 1; attempt <= MaxReshuffleAttempts; attempt++)
            {
                ShuffleBoard(boardModel);
                bool isPlayableAfterShuffle = IsPlayable(boardModel);

                if (isPlayableAfterShuffle)
                {
                    return attempt;
                }
            }

            throw new InvalidOperationException("Unable to reshuffle board to a playable state.");
        }

        private bool IsPlayable(BoardModel boardModel)
        {
            List<MatchGroupModel> matchGroups = _matchRule.FindMatches(boardModel);

            if (matchGroups.Count > 0)
            {
                return false;
            }

            MoveModel possibleMoveModel;
            bool hasPossibleMove = _possibleMoveFinder.TryFindAnyMove(boardModel, out possibleMoveModel);

            return hasPossibleMove;
        }

        private void ShuffleBoard(BoardModel boardModel)
        {
            List<BoardCoordinate> movableCoordinates = new List<BoardCoordinate>();
            List<TileModel> movableTiles = new List<TileModel>();

            CollectMovableTiles(boardModel, movableCoordinates, movableTiles);

            if (movableTiles.Count <= 1)
            {
                return;
            }

            ShuffleTiles(movableTiles);
            ApplyShuffledTiles(boardModel, movableCoordinates, movableTiles);
        }

        private static void CollectMovableTiles(BoardModel boardModel, List<BoardCoordinate> movableCoordinates, List<TileModel> movableTiles)
        {
            for (int row = 0; row < boardModel.Height; row++)
            {
                for (int column = 0; column < boardModel.Width; column++)
                {
                    BoardCoordinate coordinate = new BoardCoordinate(column, row);
                    CellModel cell = boardModel.GetCell(coordinate);

                    if (cell.IsBlocked)
                    {
                        continue;
                    }

                    if (cell.IsEmpty)
                    {
                        continue;
                    }

                    movableCoordinates.Add(coordinate);
                    movableTiles.Add(cell.Tile);
                }
            }
        }

        private void ShuffleTiles(List<TileModel> movableTiles)
        {
            for (int index = movableTiles.Count - 1; index > 0; index--)
            {
                int otherIndex = _randomProvider.Range(0, index + 1);
                TileModel currentTile = movableTiles[index];
                TileModel otherTile = movableTiles[otherIndex];
                movableTiles[index] = otherTile;
                movableTiles[otherIndex] = currentTile;
            }
        }

        private static void ApplyShuffledTiles(BoardModel boardModel, List<BoardCoordinate> movableCoordinates, List<TileModel> movableTiles)
        {
            for (int index = 0; index < movableCoordinates.Count; index++)
            {
                BoardCoordinate coordinate = movableCoordinates[index];
                TileModel tileModel = movableTiles[index];
                boardModel.SetTile(coordinate, tileModel);
            }
        }
    }
}
