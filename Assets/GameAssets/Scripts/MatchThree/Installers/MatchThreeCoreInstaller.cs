using MatchThree.Application;
using MatchThree.Domain.Contracts;
using MatchThree.Domain.Rules;
using MatchThree.Infrastructure;
using Zenject;

namespace MatchThree.Installers
{
    public class MatchThreeCoreInstaller : Installer<MatchThreeCoreInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IRandomProvider>().To<SystemRandomProvider>().AsSingle();
            Container.Bind<ITileGenerator>().To<UniformTileGenerator>().AsSingle();
            Container.Bind<IMoveValidator>().To<AdjacentMoveValidator>().AsSingle();
            Container.Bind<IMatchRule>().To<LineMatchRule>().AsSingle();
            Container.Bind<BoardInitializer>().AsSingle();
            Container.Bind<CascadeProcessor>().AsSingle();
            Container.Bind<MoveProcessor>().AsSingle();
            Container.Bind<MatchThreeGameService>().AsSingle();
        }
    }
}
