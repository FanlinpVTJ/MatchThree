using MatchThree.Domain;

namespace MatchThree.Application.Events
{
    public readonly struct ObstacleDamagedEventModel
    {
        public BoardCoordinate Coordinate { get; }

        public int DamageAmount { get; }

        public int RemainingDurability { get; }

        public ObstacleDamagedEventModel(BoardCoordinate coordinate, int damageAmount, int remainingDurability)
        {
            Coordinate = coordinate;
            DamageAmount = damageAmount;
            RemainingDurability = remainingDurability;
        }
    }
}
