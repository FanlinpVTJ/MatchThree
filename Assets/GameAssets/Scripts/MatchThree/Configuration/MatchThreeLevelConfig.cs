using MatchThree.Domain;
using UnityEngine;

namespace MatchThree.Configuration
{
    [CreateAssetMenu(fileName = "MatchThreeLevelConfig", menuName = "MatchThree/Level Config")]
    public class MatchThreeLevelConfig : ScriptableObject
    {
        [SerializeField]
        private int _boardWidth = 8;

        [SerializeField]
        private int _boardHeight = 8;

        [SerializeField]
        private bool _runMoveOnStart = true;

        [SerializeField]
        private int _fromColumn;

        [SerializeField]
        private int _fromRow;

        [SerializeField]
        private int _toColumn = 1;

        [SerializeField]
        private int _toRow;

        [SerializeField]
        private bool _useDeterministicSeed;

        [SerializeField]
        private int _deterministicSeed;

        [SerializeField]
        private TileColorType[] _availableColorTypes =
        {
            TileColorType.Red,
            TileColorType.Green,
            TileColorType.Blue,
            TileColorType.Yellow,
            TileColorType.Purple,
            TileColorType.Orange
        };

        [SerializeField]
        private int _moveLimit = 30;

        [SerializeField]
        private int _targetMatchGroupCount = 10;

        [SerializeField]
        private BlockedCellConfigModel[] _blockedCells = new BlockedCellConfigModel[0];

        public int BoardWidth
        {
            get
            {
                return _boardWidth;
            }
        }

        public int BoardHeight
        {
            get
            {
                return _boardHeight;
            }
        }

        public bool RunMoveOnStart
        {
            get
            {
                return _runMoveOnStart;
            }
        }

        public int FromColumn
        {
            get
            {
                return _fromColumn;
            }
        }

        public int FromRow
        {
            get
            {
                return _fromRow;
            }
        }

        public int ToColumn
        {
            get
            {
                return _toColumn;
            }
        }

        public int ToRow
        {
            get
            {
                return _toRow;
            }
        }

        public bool UseDeterministicSeed
        {
            get
            {
                return _useDeterministicSeed;
            }
        }

        public int DeterministicSeed
        {
            get
            {
                return _deterministicSeed;
            }
        }

        public TileColorType[] AvailableColorTypes
        {
            get
            {
                return _availableColorTypes;
            }
        }

        public int MoveLimit
        {
            get
            {
                return _moveLimit;
            }
        }

        public int TargetMatchGroupCount
        {
            get
            {
                return _targetMatchGroupCount;
            }
        }

        public BlockedCellConfigModel[] BlockedCells
        {
            get
            {
                return _blockedCells;
            }
        }
    }
}
