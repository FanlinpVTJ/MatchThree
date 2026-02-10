using System.Collections.Generic;
using MatchThree.Domain.Contracts;

namespace MatchThree.Domain.Rules
{
    public class LineMatchRule : IMatchRule
    {
        public List<MatchGroupModel> FindMatches(BoardModel boardModel)
        {
            List<MatchGroupModel> matchGroups = new List<MatchGroupModel>();

            FindHorizontalMatches(boardModel, matchGroups);
            FindVerticalMatches(boardModel, matchGroups);

            return matchGroups;
        }

        private static void FindHorizontalMatches(BoardModel boardModel, List<MatchGroupModel> matchGroups)
        {
            for (int row = 0; row < boardModel.Height; row++)
            {
                int column = 0;

                while (column < boardModel.Width)
                {
                    BoardCoordinate startCoordinate = new BoardCoordinate(column, row);
                    CellModel startCell = boardModel.GetCell(startCoordinate);

                    if (startCell.IsEmpty || startCell.IsBlocked)
                    {
                        column++;

                        continue;
                    }

                    TileColorType colorType = startCell.Tile.ColorType;
                    int matchLength = 1;
                    int nextColumn = column + 1;

                    while (nextColumn < boardModel.Width)
                    {
                        BoardCoordinate nextCoordinate = new BoardCoordinate(nextColumn, row);
                        CellModel nextCell = boardModel.GetCell(nextCoordinate);

                        if (nextCell.IsEmpty || nextCell.IsBlocked)
                        {
                            break;
                        }

                        if (nextCell.Tile.ColorType != colorType)
                        {
                            break;
                        }

                        matchLength++;
                        nextColumn++;
                    }

                    if (matchLength >= 3)
                    {
                        List<BoardCoordinate> coordinates = new List<BoardCoordinate>();

                        for (int matchColumn = column; matchColumn < column + matchLength; matchColumn++)
                        {
                            BoardCoordinate coordinate = new BoardCoordinate(matchColumn, row);
                            coordinates.Add(coordinate);
                        }

                        MatchGroupModel group = new MatchGroupModel(coordinates, colorType);
                        matchGroups.Add(group);
                    }

                    column += matchLength;
                }
            }
        }

        private static void FindVerticalMatches(BoardModel boardModel, List<MatchGroupModel> matchGroups)
        {
            for (int column = 0; column < boardModel.Width; column++)
            {
                int row = 0;

                while (row < boardModel.Height)
                {
                    BoardCoordinate startCoordinate = new BoardCoordinate(column, row);
                    CellModel startCell = boardModel.GetCell(startCoordinate);

                    if (startCell.IsEmpty || startCell.IsBlocked)
                    {
                        row++;

                        continue;
                    }

                    TileColorType colorType = startCell.Tile.ColorType;
                    int matchLength = 1;
                    int nextRow = row + 1;

                    while (nextRow < boardModel.Height)
                    {
                        BoardCoordinate nextCoordinate = new BoardCoordinate(column, nextRow);
                        CellModel nextCell = boardModel.GetCell(nextCoordinate);

                        if (nextCell.IsEmpty || nextCell.IsBlocked)
                        {
                            break;
                        }

                        if (nextCell.Tile.ColorType != colorType)
                        {
                            break;
                        }

                        matchLength++;
                        nextRow++;
                    }

                    if (matchLength >= 3)
                    {
                        List<BoardCoordinate> coordinates = new List<BoardCoordinate>();

                        for (int matchRow = row; matchRow < row + matchLength; matchRow++)
                        {
                            BoardCoordinate coordinate = new BoardCoordinate(column, matchRow);
                            coordinates.Add(coordinate);
                        }

                        MatchGroupModel group = new MatchGroupModel(coordinates, colorType);
                        matchGroups.Add(group);
                    }

                    row += matchLength;
                }
            }
        }
    }
}
