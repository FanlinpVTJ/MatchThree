using UnityEngine;
using Zenject;

public sealed class InventoryScreenInstaller : MonoInstaller
{
    [SerializeField] private InventoryScreenView _inventoryScreenView;

    public override void InstallBindings()
    {
        Container.Bind<IInventoryRepository>()
            .To<InMemoryInventoryRepository>()
            .AsSingle();

        Container.Bind<IInventoryApplicationService>()
            .To<InventoryApplicationService>()
            .AsSingle();

        Container.Bind<IInventoryModel>()
            .To<InventoryModel>()
            .AsSingle();

        Container.Bind<IInventoryScreenViewModel>()
            .To<InventoryScreenViewModel>()
            .AsSingle();

        Container.Bind<IInventoryScreenAnimator>()
            .To<InventoryScreenAnimator>()
            .AsSingle();

        Container.Bind<IInventoryScreenView>()
            .FromInstance(_inventoryScreenView)
            .AsSingle();
    }
}