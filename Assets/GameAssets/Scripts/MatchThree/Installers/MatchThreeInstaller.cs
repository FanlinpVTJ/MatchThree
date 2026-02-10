using MatchThree.Application;
using MatchThree.Application.Events;
using MatchThree.Domain.Contracts;
using MatchThree.Domain.Rules;
using MatchThree.Infrastructure;
using MessagePipe;
using Zenject;

namespace MatchThree.Installers
{
    public class MatchThreeInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            MessagePipeOptions messagePipeOptions = Container.BindMessagePipe();
            Container.BindMessageBroker<MoveStartedEventModel>(messagePipeOptions);
            Container.BindMessageBroker<MoveRejectedEventModel>(messagePipeOptions);
            Container.BindMessageBroker<MoveAppliedEventModel>(messagePipeOptions);
            Container.BindMessageBroker<CascadeResolvedEventModel>(messagePipeOptions);
            Container.BindMessageBroker<BoardSettledEventModel>(messagePipeOptions);

            Container.Bind<IRandomProvider>().To<SystemRandomProvider>().AsSingle();
            Container.Bind<ITileGenerator>().To<UniformTileGenerator>().AsSingle();
            Container.Bind<IMoveValidator>().To<AdjacentMoveValidator>().AsSingle();
            Container.Bind<IMatchRule>().To<LineMatchRule>().AsSingle();
            Container.Bind<BoardInitializer>().AsSingle();
            Container.Bind<CascadeProcessor>().AsSingle();
            Container.Bind<MoveProcessor>().AsSingle();
        }
    }
}
