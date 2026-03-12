using MatchThree.Domain.Interfaces;
using MatchThree.Domain.Rules;
using MatchThree.Infrastructure.Factories;
using Zenject;

namespace MatchThree.Infrastructure.Installers
{
    public class MatchThreeProjectContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IBoardInitializationRule>().To<DefaultBoardInitializationRule>().AsSingle();
            Container.Bind<ISwapValidationRule>().To<DefaultSwapValidationRule>().AsSingle();
            Container.Bind<IMatchDetectionRule>().To<DefaultMatchDetectionRule>().AsSingle();

            Container.Bind<IBoardModelFactory>().To<BoardModelFactory>().AsSingle();
            Container.Bind<ICellModelFactory>().To<CellModelFactory>().AsSingle();
            Container.Bind<IPieceModelFactory>().To<PieceModelFactory>().AsSingle();

            Container.Bind<GameSessionViewModelFactory>().AsSingle();
            Container.Bind<BoardViewModelFactory>().AsSingle();
            Container.Bind<CellViewModelFactory>().AsSingle();
            Container.Bind<PieceViewModelFactory>().AsSingle();
        }
    }
}
