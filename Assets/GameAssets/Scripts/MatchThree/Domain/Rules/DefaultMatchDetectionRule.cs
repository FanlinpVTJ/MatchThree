using System.Collections.Generic;
using MatchThree.Domain.Interfaces;
using MatchThree.Domain.Models;
using MatchThree.Domain.ValueObjects;

namespace MatchThree.Domain.Rules
{
    public class DefaultMatchDetectionRule : IMatchDetectionRule
    {
        public List<MatchGroupModel> FindMatchGroupModels(BoardModel boardModel)
        {
            List<MatchGroupModel> matchGroupModels = new List<MatchGroupModel>();
            FindHorizontalMatchGroupModels(boardModel, matchGroupModels);
            FindVerticalMatchGroupModels(boardModel, matchGroupModels);

            return matchGroupModels;
        }

        private void FindHorizontalMatchGroupModels(BoardModel boardModel, List<MatchGroupModel> matchGroupModels)
        {
            for (int rowIndex = 0; rowIndex < boardModel.HeightValue; rowIndex++)
            {
                int runPieceTypeIdentifier = 0;
                List<GridPositionModel> runPositionModels = new List<GridPositionModel>();

                for (int columnIndex = 0; columnIndex < boardModel.WidthValue; columnIndex++)
                {
                    GridPositionModel positionModel = new GridPositionModel(rowIndex, columnIndex);
                    boardModel.TryGetCellModel(positionModel, out CellModel cellModel);
                    int currentPieceTypeIdentifier = GetPieceTypeIdentifier(cellModel);

                    if (currentPieceTypeIdentifier != 0 && currentPieceTypeIdentifier == runPieceTypeIdentifier)
                    {
                        runPositionModels.Add(positionModel);
                    }
                    else
                    {
                        AddMatchGroupModelIfNeeded(runPieceTypeIdentifier, runPositionModels, matchGroupModels);
                        runPositionModels = new List<GridPositionModel>();
                        runPieceTypeIdentifier = currentPieceTypeIdentifier;

                        if (currentPieceTypeIdentifier != 0)
                        {
                            runPositionModels.Add(positionModel);
                        }
                    }
                }

                AddMatchGroupModelIfNeeded(runPieceTypeIdentifier, runPositionModels, matchGroupModels);
            }
        }

        private void FindVerticalMatchGroupModels(BoardModel boardModel, List<MatchGroupModel> matchGroupModels)
        {
            for (int columnIndex = 0; columnIndex < boardModel.WidthValue; columnIndex++)
            {
                int runPieceTypeIdentifier = 0;
                List<GridPositionModel> runPositionModels = new List<GridPositionModel>();

                for (int rowIndex = 0; rowIndex < boardModel.HeightValue; rowIndex++)
                {
                    GridPositionModel positionModel = new GridPositionModel(rowIndex, columnIndex);
                    boardModel.TryGetCellModel(positionModel, out CellModel cellModel);
                    int currentPieceTypeIdentifier = GetPieceTypeIdentifier(cellModel);

                    if (currentPieceTypeIdentifier != 0 && currentPieceTypeIdentifier == runPieceTypeIdentifier)
                    {
                        runPositionModels.Add(positionModel);
                    }
                    else
                    {
                        AddMatchGroupModelIfNeeded(runPieceTypeIdentifier, runPositionModels, matchGroupModels);
                        runPositionModels = new List<GridPositionModel>();
                        runPieceTypeIdentifier = currentPieceTypeIdentifier;

                        if (currentPieceTypeIdentifier != 0)
                        {
                            runPositionModels.Add(positionModel);
                        }
                    }
                }

                AddMatchGroupModelIfNeeded(runPieceTypeIdentifier, runPositionModels, matchGroupModels);
            }
        }

        private void AddMatchGroupModelIfNeeded(
            int pieceTypeIdentifier,
            List<GridPositionModel> runPositionModels,
            List<MatchGroupModel> matchGroupModels)
        {
            if (pieceTypeIdentifier == 0)
            {
                return;
            }

            if (runPositionModels.Count < 3)
            {
                return;
            }

            List<GridPositionModel> matchPositionModels = new List<GridPositionModel>(runPositionModels);
            MatchGroupModel matchGroupModel = new MatchGroupModel(pieceTypeIdentifier, matchPositionModels);
            matchGroupModels.Add(matchGroupModel);
        }

        private int GetPieceTypeIdentifier(CellModel cellModel)
        {
            if (cellModel == null || cellModel.PieceModel == null)
            {
                return 0;
            }

            return cellModel.PieceModel.PieceTypeIdentifier;
        }
    }
}
