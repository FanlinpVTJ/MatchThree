using System.Collections.Generic;
using MatchThree.Domain.Interfaces;
using MatchThree.Domain.Models;

namespace MatchThree.Application.Services
{
    public class BoardSwapService : IBoardSwapService
    {
        private readonly IBoardModelFactory _boardModelFactory;
        private readonly ICellModelFactory _cellModelFactory;
        private readonly IPieceModelFactory _pieceModelFactory;

        public BoardSwapService(
            IBoardModelFactory boardModelFactory,
            ICellModelFactory cellModelFactory,
            IPieceModelFactory pieceModelFactory)
        {
            _boardModelFactory = boardModelFactory;
            _cellModelFactory = cellModelFactory;
            _pieceModelFactory = pieceModelFactory;
        }

        public BoardModel CreateSwappedBoardModel(BoardModel boardModel, SwapCommandModel swapCommandModel)
        {
            BoardModel clonedBoardModel = CreateClonedBoardModel(boardModel);
            clonedBoardModel.TryGetCellModel(swapCommandModel.SourcePositionModel, out CellModel sourceCellModel);
            clonedBoardModel.TryGetCellModel(swapCommandModel.TargetPositionModel, out CellModel targetCellModel);

            PieceModel sourcePieceModel = sourceCellModel.PieceModel;
            PieceModel targetPieceModel = targetCellModel.PieceModel;

            if (sourcePieceModel != null)
            {
                sourcePieceModel.UpdatePositionModel(targetCellModel.PositionModel);
            }

            if (targetPieceModel != null)
            {
                targetPieceModel.UpdatePositionModel(sourceCellModel.PositionModel);
            }

            sourceCellModel.UpdatePieceModel(targetPieceModel);
            targetCellModel.UpdatePieceModel(sourcePieceModel);

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
