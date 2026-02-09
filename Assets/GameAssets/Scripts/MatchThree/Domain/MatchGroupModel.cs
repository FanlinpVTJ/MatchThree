using System.Collections.Generic;

namespace MatchThree.Domain
{
    public class MatchGroupModel
    {
        public List<BoardCoordinate> Coordinates { get; }

        public TileColorType ColorType { get; }

        public MatchGroupModel(List<BoardCoordinate> coordinates, TileColorType colorType)
        {
            Coordinates = coordinates;
            ColorType = colorType;
        }
    }
}
