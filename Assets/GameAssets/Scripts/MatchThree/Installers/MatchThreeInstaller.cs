using System;
using MatchThree.Application;
using MatchThree.Application.Events;
using MatchThree.Configuration;
using MatchThree.Domain;
using MatchThree.Domain.Contracts;
using MatchThree.Domain.Rules;
using MatchThree.Infrastructure;
using MessagePipe;
using UnityEngine;
using Zenject;

namespace MatchThree.Installers
{
    public class MatchThreeInstaller : MonoInstaller
    {
        [SerializeField]
        private MatchThreeLevelConfig _levelConfig;

        public override void InstallBindings()
        {
            MatchThreeGameSettingsModel settingsModel = CreateSettingsModel();
            Container.Bind<MatchThreeGameSettingsModel>().FromInstance(settingsModel).AsSingle();

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

        private MatchThreeGameSettingsModel CreateSettingsModel()
        {
            if (_levelConfig == null)
            {
                throw new InvalidOperationException("MatchThreeLevelConfig is not assigned.");
            }

            TileColorType[] configuredColorTypes = _levelConfig.AvailableColorTypes;

            if (configuredColorTypes == null || configuredColorTypes.Length == 0)
            {
                throw new InvalidOperationException("MatchThreeLevelConfig.AvailableColorTypes must contain at least one value.");
            }

            TileColorType[] availableColorTypes = new TileColorType[configuredColorTypes.Length];

            for (int index = 0; index < configuredColorTypes.Length; index++)
            {
                availableColorTypes[index] = configuredColorTypes[index];
            }

            MatchThreeGameSettingsModel settingsModel = new MatchThreeGameSettingsModel(
                _levelConfig.BoardWidth,
                _levelConfig.BoardHeight,
                _levelConfig.RunMoveOnStart,
                _levelConfig.FromColumn,
                _levelConfig.FromRow,
                _levelConfig.ToColumn,
                _levelConfig.ToRow,
                _levelConfig.UseDeterministicSeed,
                _levelConfig.DeterministicSeed,
                availableColorTypes);

            return settingsModel;
        }
    }
}
