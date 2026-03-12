using MatchThree.Presentation.ViewModels;
using Zenject;

namespace MatchThree.Infrastructure.Factories
{
    public class BoardViewModelFactory
    {
        private readonly IInstantiator _instantiator;

        public BoardViewModelFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public BoardViewModel CreateBoardViewModel()
        {
            BoardViewModel boardViewModel = _instantiator.Instantiate<BoardViewModel>();

            return boardViewModel;
        }
    }
}
