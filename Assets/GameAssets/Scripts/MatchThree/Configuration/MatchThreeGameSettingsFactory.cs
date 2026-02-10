using System;
using System.Collections.Generic;
using MatchThree.Application;
using MatchThree.Domain;

namespace MatchThree.Configuration
{
    public class MatchThreeGameSettingsFactory
    {
        public MatchThreeGameSettingsModel Create(MatchThreeLevelConfig levelConfig)
        {
            if (levelConfig == null)
            {
                throw new InvalidOperationException("MatchThreeLevelConfig is not assigned.");
            }

            ValidateLevelConfig(levelConfig);

            TileColorType[] configuredColorTypes = levelConfig.AvailableColorTypes;
            TileColorType[] availableColorTypes = new TileColorType[configuredColorTypes.Length];

            for (int index = 0; index < configuredColorTypes.Length; index++)
            {
                availableColorTypes[index] = configuredColorTypes[index];
            }

            BlockedCellModel[] blockedCells = CreateBlockedCells(levelConfig);
            MatchThreeGameSettingsModel settingsModel = new MatchThreeGameSettingsModel(
                levelConfig.BoardWidth,
                levelConfig.BoardHeight,
                levelConfig.RunMoveOnStart,
                levelConfig.FromColumn,
                levelConfig.FromRow,
                levelConfig.ToColumn,
                levelConfig.ToRow,
                levelConfig.UseDeterministicSeed,
                levelConfig.DeterministicSeed,
                availableColorTypes,
                levelConfig.MoveLimit,
                levelConfig.TargetMatchGroupCount,
                blockedCells);

            return settingsModel;
        }

        private BlockedCellModel[] CreateBlockedCells(MatchThreeLevelConfig levelConfig)
        {
            BlockedCellConfigModel[] configuredBlockedCells = levelConfig.BlockedCells;

            if (configuredBlockedCells == null || configuredBlockedCells.Length == 0)
            {
                BlockedCellModel[] emptyBlockedCells = new BlockedCellModel[0];

                return emptyBlockedCells;
            }

            List<BlockedCellModel> blockedCells = new List<BlockedCellModel>(configuredBlockedCells.Length);

            for (int index = 0; index < configuredBlockedCells.Length; index++)
            {
                BlockedCellConfigModel blockedCellConfigModel = configuredBlockedCells[index];
                BlockedCellModel blockedCellModel = new BlockedCellModel(
                    blockedCellConfigModel.Column,
                    blockedCellConfigModel.Row,
                    blockedCellConfigModel.BlockType,
                    blockedCellConfigModel.Durability);
                blockedCells.Add(blockedCellModel);
            }

            BlockedCellModel[] blockedCellsArray = blockedCells.ToArray();

            return blockedCellsArray;
        }

        private void ValidateLevelConfig(MatchThreeLevelConfig levelConfig)
        {
            if (levelConfig.BoardWidth <= 0 || levelConfig.BoardHeight <= 0)
            {
                throw new InvalidOperationException("Board size must be greater than zero.");
            }

            if (!IsCoordinateInBounds(levelConfig.FromColumn, levelConfig.FromRow, levelConfig))
            {
                throw new InvalidOperationException("From coordinate is outside board bounds.");
            }

            if (!IsCoordinateInBounds(levelConfig.ToColumn, levelConfig.ToRow, levelConfig))
            {
                throw new InvalidOperationException("To coordinate is outside board bounds.");
            }

            TileColorType[] configuredColorTypes = levelConfig.AvailableColorTypes;

            if (configuredColorTypes == null || configuredColorTypes.Length == 0)
            {
                throw new InvalidOperationException("MatchThreeLevelConfig.AvailableColorTypes must contain at least one value.");
            }

            if (levelConfig.MoveLimit <= 0)
            {
                throw new InvalidOperationException("MoveLimit must be greater than zero.");
            }

            if (levelConfig.TargetMatchGroupCount <= 0)
            {
                throw new InvalidOperationException("TargetMatchGroupCount must be greater than zero.");
            }

            ValidateBlockedCells(levelConfig);
        }

        private void ValidateBlockedCells(MatchThreeLevelConfig levelConfig)
        {
            BlockedCellConfigModel[] blockedCells = levelConfig.BlockedCells;

            if (blockedCells == null || blockedCells.Length == 0)
            {
                return;
            }

            bool[,] occupiedCoordinates = new bool[levelConfig.BoardWidth, levelConfig.BoardHeight];

            for (int index = 0; index < blockedCells.Length; index++)
            {
                BlockedCellConfigModel blockedCellConfigModel = blockedCells[index];

                if (blockedCellConfigModel.BlockType == CellBlockType.None)
                {
                    continue;
                }

                if (blockedCellConfigModel.BlockType == CellBlockType.Durable && blockedCellConfigModel.Durability <= 0)
                {
                    throw new InvalidOperationException("Durable blocked cell must have durability greater than zero.");
                }

                if (!IsCoordinateInBounds(blockedCellConfigModel.Column, blockedCellConfigModel.Row, levelConfig))
                {
                    throw new InvalidOperationException("Blocked cell coordinate is outside board bounds.");
                }

                if (occupiedCoordinates[blockedCellConfigModel.Column, blockedCellConfigModel.Row])
                {
                    throw new InvalidOperationException("Blocked cells contain duplicate coordinates.");
                }

                occupiedCoordinates[blockedCellConfigModel.Column, blockedCellConfigModel.Row] = true;
            }
        }

        private bool IsCoordinateInBounds(int column, int row, MatchThreeLevelConfig levelConfig)
        {
            bool isColumnInBounds = column >= 0 && column < levelConfig.BoardWidth;
            bool isRowInBounds = row >= 0 && row < levelConfig.BoardHeight;

            return isColumnInBounds && isRowInBounds;
        }
    }
}
