using MatchThree.Application;
using MatchThree.Configuration;
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
            MatchThreeGameSettingsFactory settingsFactory = new MatchThreeGameSettingsFactory();
            MatchThreeGameSettingsModel settingsModel = settingsFactory.Create(_levelConfig);
            Container.Bind<MatchThreeGameSettingsModel>().FromInstance(settingsModel).AsSingle();

            MatchThreeMessagingInstaller.Install(Container);
            MatchThreeCoreInstaller.Install(Container);
        }
    }
}
