using MatchThree.Application.Events;
using MessagePipe;
using Zenject;

namespace MatchThree.Installers
{
    public class MatchThreeMessagingInstaller : Installer<MatchThreeMessagingInstaller>
    {
        public override void InstallBindings()
        {
            MessagePipeOptions messagePipeOptions = Container.BindMessagePipe();
            Container.BindMessageBroker<MoveStartedEventModel>(messagePipeOptions);
            Container.BindMessageBroker<MoveRejectedEventModel>(messagePipeOptions);
            Container.BindMessageBroker<MoveAppliedEventModel>(messagePipeOptions);
            Container.BindMessageBroker<CascadeResolvedEventModel>(messagePipeOptions);
            Container.BindMessageBroker<BoardSettledEventModel>(messagePipeOptions);
            Container.BindMessageBroker<GameProgressChangedEventModel>(messagePipeOptions);
            Container.BindMessageBroker<GameFinishedEventModel>(messagePipeOptions);
        }
    }
}
