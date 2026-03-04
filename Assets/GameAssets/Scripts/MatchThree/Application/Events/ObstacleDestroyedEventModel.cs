using MatchThree.Domain;

namespace MatchThree.Application.Events
{
    public readonly struct ObstacleDestroyedEventModel
    {
        public BoardCoordinate Coordinate { get; }

        public ObstacleDestroyedEventModel(BoardCoordinate coordinate)
        {
            Coordinate = coordinate;
        }
    }
}
