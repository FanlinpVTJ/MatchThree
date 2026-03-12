using MatchThree.Presentation.ViewModels;
using Zenject;

namespace MatchThree.Infrastructure.Factories
{
    public class GameSessionViewModelFactory
    {
        private readonly IInstantiator _instantiator;

        public GameSessionViewModelFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public GameSessionViewModel CreateGameSessionViewModel()
        {
            GameSessionViewModel gameSessionViewModel = _instantiator.Instantiate<GameSessionViewModel>();

            return gameSessionViewModel;
        }
    }
}
