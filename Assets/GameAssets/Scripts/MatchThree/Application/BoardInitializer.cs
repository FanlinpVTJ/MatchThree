using MatchThree.Domain;
using MatchThree.Domain.Contracts;

namespace MatchThree.Application
{
    public class BoardInitializer
    {
        private readonly ITileGenerator _tileGenerator;

        private readonly MatchThreeGameSettingsModel _settingsModel;

        public BoardInitializer(ITileGenerator tileGenerator, MatchThreeGameSettingsModel settingsModel)
        {
            _tileGenerator = tileGenerator;
            _settingsModel = settingsModel;
        }

        public BoardModel CreateBoard(int width, int height)
        {
            BoardModel boardModel = new BoardModel(width, height);
            ApplyBlockedCells(boardModel);
            FillBoard(boardModel);

            return boardModel;
        }

        public void FillBoard(BoardModel boardModel)
        {
            for (int row = 0; row < boardModel.Height; row++)
            {
                for (int column = 0; column < boardModel.Width; column++)
                {
                    BoardCoordinate coordinate = new BoardCoordinate(column, row);
                    CellModel cellModel = boardModel.GetCell(coordinate);

                    if (cellModel.IsBlocked)
                    {
                        continue;
                    }

                    TileModel tileModel = _tileGenerator.CreateTile();
                    boardModel.SetTile(coordinate, tileModel);
                }
            }
        }

        private void ApplyBlockedCells(BoardModel boardModel)
        {
            BlockedCellModel[] blockedCells = _settingsModel.BlockedCells;

            for (int index = 0; index < blockedCells.Length; index++)
            {
                BlockedCellModel blockedCellModel = blockedCells[index];

                if (blockedCellModel.BlockType == CellBlockType.None)
                {
                    continue;
                }

                BoardCoordinate coordinate = new BoardCoordinate(blockedCellModel.Column, blockedCellModel.Row);
                boardModel.SetBlocked(coordinate, true);
            }
        }
    }
}
