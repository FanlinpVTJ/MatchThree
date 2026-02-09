namespace MatchThree.Domain.Contracts
{
    public interface IRandomProvider
    {
        int Range(int minInclusive, int maxExclusive);
    }
}
