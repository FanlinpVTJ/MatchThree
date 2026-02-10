using MatchThree.Domain;
using MatchThree.Domain.Contracts;

namespace MatchThree.Infrastructure
{
    public class UniformTileGenerator : ITileGenerator
    {
        private static readonly TileColorType[] AvailableColorTypes =
        {
            TileColorType.Red,
            TileColorType.Green,
            TileColorType.Blue,
            TileColorType.Yellow,
            TileColorType.Purple,
            TileColorType.Orange
        };

        private readonly IRandomProvider _randomProvider;

        public UniformTileGenerator(IRandomProvider randomProvider)
        {
            _randomProvider = randomProvider;
        }

        public TileModel CreateTile()
        {
            int index = _randomProvider.Range(0, AvailableColorTypes.Length);
            TileColorType colorType = AvailableColorTypes[index];
            TileModel tileModel = new TileModel(colorType);

            return tileModel;
        }
    }
}
