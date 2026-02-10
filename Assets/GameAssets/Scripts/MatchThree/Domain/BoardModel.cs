using System;

namespace MatchThree.Domain
{
    public class BoardModel
    {
        private readonly CellModel[,] _cells;

        public int Width { get; }

        public int Height { get; }

        public BoardModel(int width, int height)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }

            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }

            Width = width;
            Height = height;
            _cells = new CellModel[width, height];

            InitializeCells();
        }

        public CellModel GetCell(BoardCoordinate coordinate)
        {
            EnsureCoordinateInBounds(coordinate);

            return _cells[coordinate.Column, coordinate.Row];
        }

        public bool IsCoordinateInBounds(BoardCoordinate coordinate)
        {
            bool isColumnInBounds = coordinate.Column >= 0 && coordinate.Column < Width;
            bool isRowInBounds = coordinate.Row >= 0 && coordinate.Row < Height;

            return isColumnInBounds && isRowInBounds;
        }

        public void SetTile(BoardCoordinate coordinate, TileModel tile)
        {
            EnsureCoordinateInBounds(coordinate);

            CellModel cell = _cells[coordinate.Column, coordinate.Row];
            cell.SetTile(tile);
        }

        public void ClearTile(BoardCoordinate coordinate)
        {
            EnsureCoordinateInBounds(coordinate);

            CellModel cell = _cells[coordinate.Column, coordinate.Row];
            cell.ClearTile();
        }

        public void SetBlocked(BoardCoordinate coordinate, CellBlockType blockType, int blockDurability)
        {
            EnsureCoordinateInBounds(coordinate);

            CellModel cell = _cells[coordinate.Column, coordinate.Row];
            cell.SetBlocked(blockType, blockDurability);

            if (blockType != CellBlockType.None)
            {
                cell.ClearTile();
            }
        }

        public void SwapTiles(BoardCoordinate firstCoordinate, BoardCoordinate secondCoordinate)
        {
            EnsureCoordinateInBounds(firstCoordinate);
            EnsureCoordinateInBounds(secondCoordinate);

            CellModel firstCell = _cells[firstCoordinate.Column, firstCoordinate.Row];
            CellModel secondCell = _cells[secondCoordinate.Column, secondCoordinate.Row];
            TileModel firstTile = firstCell.Tile;
            TileModel secondTile = secondCell.Tile;

            firstCell.SetTile(secondTile);
            secondCell.SetTile(firstTile);
        }

        private void InitializeCells()
        {
            for (int row = 0; row < Height; row++)
            {
                for (int column = 0; column < Width; column++)
                {
                    BoardCoordinate coordinate = new BoardCoordinate(column, row);
                    CellModel cell = new CellModel(coordinate, null, false);
                    _cells[column, row] = cell;
                }
            }
        }

        private void EnsureCoordinateInBounds(BoardCoordinate coordinate)
        {
            if (IsCoordinateInBounds(coordinate))
            {
                return;
            }

            throw new ArgumentOutOfRangeException(nameof(coordinate));
        }
    }
}
