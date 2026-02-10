using System.Collections.Generic;
using MatchThree.Application.Events;
using MatchThree.Domain;
using MatchThree.Domain.Contracts;
using MessagePipe;

namespace MatchThree.Application
{
    public class CascadeProcessor
    {
        private readonly IMatchRule _matchRule;

        private readonly ITileGenerator _tileGenerator;

        private readonly IPublisher<CascadeResolvedEventModel> _cascadeResolvedEventPublisher;

        public CascadeProcessor(
            IMatchRule matchRule,
            ITileGenerator tileGenerator,
            IPublisher<CascadeResolvedEventModel> cascadeResolvedEventPublisher)
        {
            _matchRule = matchRule;
            _tileGenerator = tileGenerator;
            _cascadeResolvedEventPublisher = cascadeResolvedEventPublisher;
        }

        public CascadeResolveResultModel Resolve(BoardModel boardModel)
        {
            List<MatchGroupModel> allMatchGroups = new List<MatchGroupModel>();
            int cascadeCount = 0;

            while (true)
            {
                List<MatchGroupModel> currentMatchGroups = _matchRule.FindMatches(boardModel);

                if (currentMatchGroups.Count == 0)
                {
                    break;
                }

                cascadeCount++;
                allMatchGroups.AddRange(currentMatchGroups);
                CascadeResolvedEventModel cascadeEventModel = new CascadeResolvedEventModel(cascadeCount, currentMatchGroups.Count);
                _cascadeResolvedEventPublisher.Publish(cascadeEventModel);

                HashSet<BoardCoordinate> removedCoordinates = RemoveMatchedTiles(boardModel, currentMatchGroups);
                ApplyObstacleDamage(boardModel, removedCoordinates);
                CollapseTiles(boardModel);
                RefillTiles(boardModel);
            }

            CascadeResolveResultModel resultModel = new CascadeResolveResultModel(allMatchGroups, cascadeCount);

            return resultModel;
        }

        private static HashSet<BoardCoordinate> RemoveMatchedTiles(BoardModel boardModel, List<MatchGroupModel> matchGroups)
        {
            HashSet<BoardCoordinate> uniqueCoordinates = new HashSet<BoardCoordinate>();

            for (int index = 0; index < matchGroups.Count; index++)
            {
                MatchGroupModel groupModel = matchGroups[index];

                for (int coordinateIndex = 0; coordinateIndex < groupModel.Coordinates.Count; coordinateIndex++)
                {
                    BoardCoordinate coordinate = groupModel.Coordinates[coordinateIndex];

                    if (uniqueCoordinates.Add(coordinate))
                    {
                        boardModel.ClearTile(coordinate);
                    }
                }
            }

            return uniqueCoordinates;
        }

        private static void ApplyObstacleDamage(BoardModel boardModel, HashSet<BoardCoordinate> removedCoordinates)
        {
            Dictionary<BoardCoordinate, int> damageByCoordinate = new Dictionary<BoardCoordinate, int>();

            foreach (BoardCoordinate removedCoordinate in removedCoordinates)
            {
                AccumulateDamage(boardModel, removedCoordinate.Column - 1, removedCoordinate.Row, damageByCoordinate);
                AccumulateDamage(boardModel, removedCoordinate.Column + 1, removedCoordinate.Row, damageByCoordinate);
                AccumulateDamage(boardModel, removedCoordinate.Column, removedCoordinate.Row - 1, damageByCoordinate);
                AccumulateDamage(boardModel, removedCoordinate.Column, removedCoordinate.Row + 1, damageByCoordinate);
            }

            foreach (KeyValuePair<BoardCoordinate, int> damagePair in damageByCoordinate)
            {
                BoardCoordinate coordinate = damagePair.Key;
                int damage = damagePair.Value;
                CellModel cell = boardModel.GetCell(coordinate);
                cell.ApplyDamage(damage);
            }
        }

        private static void AccumulateDamage(BoardModel boardModel, int column, int row, Dictionary<BoardCoordinate, int> damageByCoordinate)
        {
            BoardCoordinate coordinate = new BoardCoordinate(column, row);

            if (!boardModel.IsCoordinateInBounds(coordinate))
            {
                return;
            }

            CellModel cell = boardModel.GetCell(coordinate);

            if (!cell.CanReceiveDamage())
            {
                return;
            }

            if (damageByCoordinate.ContainsKey(coordinate))
            {
                damageByCoordinate[coordinate] = damageByCoordinate[coordinate] + 1;
            }
            else
            {
                damageByCoordinate.Add(coordinate, 1);
            }
        }

        private static void CollapseTiles(BoardModel boardModel)
        {
            for (int column = 0; column < boardModel.Width; column++)
            {
                int writeRow = 0;

                for (int row = 0; row < boardModel.Height; row++)
                {
                    BoardCoordinate sourceCoordinate = new BoardCoordinate(column, row);
                    CellModel sourceCell = boardModel.GetCell(sourceCoordinate);

                    if (sourceCell.IsBlocked)
                    {
                        writeRow = row + 1;

                        continue;
                    }

                    if (sourceCell.IsEmpty)
                    {
                        continue;
                    }

                    if (writeRow != row)
                    {
                        BoardCoordinate destinationCoordinate = new BoardCoordinate(column, writeRow);
                        TileModel sourceTile = sourceCell.Tile;
                        boardModel.SetTile(destinationCoordinate, sourceTile);
                        sourceCell.ClearTile();
                    }

                    writeRow++;
                }
            }
        }

        private void RefillTiles(BoardModel boardModel)
        {
            for (int column = 0; column < boardModel.Width; column++)
            {
                for (int row = 0; row < boardModel.Height; row++)
                {
                    BoardCoordinate coordinate = new BoardCoordinate(column, row);
                    CellModel cell = boardModel.GetCell(coordinate);

                    if (cell.IsBlocked)
                    {
                        continue;
                    }

                    if (!cell.IsEmpty)
                    {
                        continue;
                    }

                    TileModel tileModel = _tileGenerator.CreateTile();
                    boardModel.SetTile(coordinate, tileModel);
                }
            }
        }
    }
}
