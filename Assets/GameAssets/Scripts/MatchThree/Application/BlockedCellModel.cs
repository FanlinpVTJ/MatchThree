using MatchThree.Domain;

namespace MatchThree.Application
{
    public readonly struct BlockedCellModel
    {
        public int Column { get; }

        public int Row { get; }

        public CellBlockType BlockType { get; }

        public int Durability { get; }

        public BlockedCellModel(int column, int row, CellBlockType blockType, int durability)
        {
            Column = column;
            Row = row;
            BlockType = blockType;
            Durability = durability;
        }
    }
}
