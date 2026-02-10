using MatchThree.Domain;
using UnityEngine;

namespace MatchThree.Configuration
{
    [System.Serializable]
    public class BlockedCellConfigModel
    {
        [SerializeField]
        private int _column;

        [SerializeField]
        private int _row;

        [SerializeField]
        private CellBlockType _blockType = CellBlockType.Solid;

        [SerializeField]
        private int _durability = 1;

        public int Column
        {
            get
            {
                return _column;
            }
        }

        public int Row
        {
            get
            {
                return _row;
            }
        }

        public CellBlockType BlockType
        {
            get
            {
                return _blockType;
            }
        }

        public int Durability
        {
            get
            {
                return _durability;
            }
        }
    }
}
