using System;
using System.Collections.Generic;
using MatchThree.Domain.Interfaces;
using MatchThree.Domain.Models;
using MatchThree.Domain.ValueObjects;

namespace MatchThree.Application.Services
{
    public class BoardInitializationService : IBoardInitializationService
    {
        private readonly IBoardInitializationRule _boardInitializationRule;
        private readonly IBoardModelFactory _boardModelFactory;
        private readonly ICellModelFactory _cellModelFactory;
        private readonly IPieceModelFactory _pieceModelFactory;

        public BoardInitializationService(
            IBoardInitializationRule boardInitializationRule,
            IBoardModelFactory boardModelFactory,
            ICellModelFactory cellModelFactory,
            IPieceModelFactory pieceModelFactory)
        {
            _boardInitializationRule = boardInitializationRule;
            _boardModelFactory = boardModelFactory;
            _cellModelFactory = cellModelFactory;
            _pieceModelFactory = pieceModelFactory;
        }

        public BoardModel CreateBoardModel(int widthValue, int heightValue, int pieceTypeCountValue)
        {
            List<CellModel> cellModels = new List<CellModel>();
            int pieceIdentifierCounter = 0;

            for (int rowIndex = 0; rowIndex < heightValue; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < widthValue; columnIndex++)
                {
                    GridPositionModel positionModel = new GridPositionModel(rowIndex, columnIndex);
                    int pieceTypeIdentifier = CalculatePieceTypeIdentifier(pieceIdentifierCounter, pieceTypeCountValue);
                    PieceModel pieceModel = _pieceModelFactory.CreatePieceModel(pieceIdentifierCounter, pieceTypeIdentifier, positionModel);
                    CellModel cellModel = _cellModelFactory.CreateCellModel(positionModel, pieceModel);
                    cellModels.Add(cellModel);
                    pieceIdentifierCounter++;
                }
            }

            BoardModel boardModel = _boardModelFactory.CreateBoardModel(widthValue, heightValue, cellModels);

            if (_boardInitializationRule.IsSatisfied(boardModel) == false)
            {
                throw new InvalidOperationException("Board initialization rule validation failed.");
            }

            return boardModel;
        }

        private int CalculatePieceTypeIdentifier(int pieceIdentifier, int pieceTypeCountValue)
        {
            if (pieceTypeCountValue <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pieceTypeCountValue));
            }

            int pieceTypeIdentifier = pieceIdentifier % pieceTypeCountValue;

            return pieceTypeIdentifier + 1;
        }
    }
}
