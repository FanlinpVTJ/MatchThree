using MatchThree.Application.Interfaces;
using MatchThree.Application.Services;
using MatchThree.Domain.Interfaces;
using MatchThree.Presentation.ViewModels;
using UnityEngine;
using Zenject;

namespace MatchThree.Infrastructure.Installers
{
    public class MatchThreeSceneContextInstaller : MonoInstaller
    {
        [SerializeField]
        private int _boardWidthValue = 8;

        [SerializeField]
        private int _boardHeightValue = 8;

        [SerializeField]
        private int _pieceTypeCountValue = 6;

        public override void InstallBindings()
        {
            Container.BindInstance(_boardWidthValue).WithId("BoardWidthValue");
            Container.BindInstance(_boardHeightValue).WithId("BoardHeightValue");
            Container.BindInstance(_pieceTypeCountValue).WithId("PieceTypeCountValue");

            Container.Bind<IBoardInitializationService>().To<BoardInitializationService>().AsSingle();
            Container.Bind<IMatch3GameSessionService>().To<Match3GameSessionService>().AsSingle();
            Container.Bind<BoardViewModel>().AsSingle();
            Container.Bind<GameSessionViewModel>().AsSingle();
        }
    }
}
