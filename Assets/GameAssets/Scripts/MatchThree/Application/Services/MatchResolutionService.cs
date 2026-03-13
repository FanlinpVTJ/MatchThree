using System.Collections.Generic;
using MatchThree.Domain.Interfaces;
using MatchThree.Domain.Models;
using MatchThree.Domain.ValueObjects;

namespace MatchThree.Application.Services
{
    public class MatchResolutionService : IMatchResolutionService
    {
        private readonly IBoardModelFactory _boardModelFactory;
        private readonly ICellModelFactory _cellModelFactory;
        private readonly IPieceModelFactory _pieceModelFactory;

        public MatchResolutionService(
            IBoardModelFactory boardModelFactory,
            ICellModelFactory cellModelFactory,
            IPieceModelFactory pieceModelFactory)
        {
            _boardModelFactory = boardModelFactory;
            _cellModelFactory = cellModelFactory;
            _pieceModelFactory = pieceModelFactory;
        }

        public BoardModel CreateResolvedBoardModel(BoardModel boardModel, List<MatchGroupModel> matchGroupModels)
        {
            BoardModel clonedBoardModel = CreateClonedBoardModel(boardModel);

            for (int matchGroupIndex = 0; matchGroupIndex < matchGroupModels.Count; matchGroupIndex++)
            {
                MatchGroupModel matchGroupModel = matchGroupModels[matchGroupIndex];

                for (int positionIndex = 0; positionIndex < matchGroupModel.PositionModels.Count; positionIndex++)
                {
                    GridPositionModel positionModel = matchGroupModel.PositionModels[positionIndex];

                    if (clonedBoardModel.TryGetCellModel(positionModel, out CellModel cellModel))
                    {
                        cellModel.UpdatePieceModel(null);
                    }
                }
            }

            return clonedBoardModel;
        }

        private BoardModel CreateClonedBoardModel(BoardModel boardModel)
        {
            List<CellModel> clonedCellModels = new List<CellModel>();

            for (int cellIndex = 0; cellIndex < boardModel.CellModels.Count; cellIndex++)
            {
                CellModel sourceCellModel = boardModel.CellModels[cellIndex];
                PieceModel clonedPieceModel = null;

                if (sourceCellModel.PieceModel != null)
                {
                    clonedPieceModel = _pieceModelFactory.CreatePieceModel(
                        sourceCellModel.PieceModel.PieceIdentifier,
                        sourceCellModel.PieceModel.PieceTypeIdentifier,
                        sourceCellModel.PieceModel.PositionModel);
                }

                CellModel clonedCellModel = _cellModelFactory.CreateCellModel(sourceCellModel.PositionModel, clonedPieceModel);
                clonedCellModels.Add(clonedCellModel);
            }

            BoardModel clonedBoardModel = _boardModelFactory.CreateBoardModel(boardModel.WidthValue, boardModel.HeightValue, clonedCellModels);

            return clonedBoardModel;
        }
    }
}
