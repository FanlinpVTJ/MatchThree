using MatchThree.Application;
using MatchThree.Domain;
using MatchThree.Domain.Contracts;

namespace MatchThree.Infrastructure
{
    public class UniformTileGenerator : ITileGenerator
    {
        private readonly IRandomProvider _randomProvider;

        private readonly TileColorType[] _availableColorTypes;

        public UniformTileGenerator(IRandomProvider randomProvider, MatchThreeGameSettingsModel settingsModel)
        {
            _randomProvider = randomProvider;
            _availableColorTypes = settingsModel.AvailableColorTypes;
        }

        public TileModel CreateTile()
        {
            int index = _randomProvider.Range(0, _availableColorTypes.Length);
            TileColorType colorType = _availableColorTypes[index];
            TileModel tileModel = new TileModel(colorType);

            return tileModel;
        }
    }
}
