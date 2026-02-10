using MatchThree.Domain;

namespace MatchThree.Application
{
    public class MatchThreeGameSettingsModel
    {
        public int BoardWidth { get; }

        public int BoardHeight { get; }

        public bool RunMoveOnStart { get; }

        public int FromColumn { get; }

        public int FromRow { get; }

        public int ToColumn { get; }

        public int ToRow { get; }

        public bool UseDeterministicSeed { get; }

        public int DeterministicSeed { get; }

        public TileColorType[] AvailableColorTypes { get; }

        public int MoveLimit { get; }

        public int TargetMatchGroupCount { get; }

        public BlockedCellModel[] BlockedCells { get; }

        public MatchThreeGameSettingsModel(
            int boardWidth,
            int boardHeight,
            bool runMoveOnStart,
            int fromColumn,
            int fromRow,
            int toColumn,
            int toRow,
            bool useDeterministicSeed,
            int deterministicSeed,
            TileColorType[] availableColorTypes,
            int moveLimit,
            int targetMatchGroupCount,
            BlockedCellModel[] blockedCells)
        {
            BoardWidth = boardWidth;
            BoardHeight = boardHeight;
            RunMoveOnStart = runMoveOnStart;
            FromColumn = fromColumn;
            FromRow = fromRow;
            ToColumn = toColumn;
            ToRow = toRow;
            UseDeterministicSeed = useDeterministicSeed;
            DeterministicSeed = deterministicSeed;
            AvailableColorTypes = availableColorTypes;
            MoveLimit = moveLimit;
            TargetMatchGroupCount = targetMatchGroupCount;
            BlockedCells = blockedCells;
        }
    }
}
