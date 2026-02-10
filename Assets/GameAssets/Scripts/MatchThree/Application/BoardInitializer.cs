using MatchThree.Domain;
using MatchThree.Domain.Contracts;

namespace MatchThree.Application
{
    public class BoardInitializer
    {
        private readonly ITileGenerator _tileGenerator;

        public BoardInitializer(ITileGenerator tileGenerator)
        {
            _tileGenerator = tileGenerator;
        }

        public BoardModel CreateBoard(int width, int height)
        {
            BoardModel boardModel = new BoardModel(width, height);
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
                    TileModel tileModel = _tileGenerator.CreateTile();
                    boardModel.SetTile(coordinate, tileModel);
                }
            }
        }
    }
}
