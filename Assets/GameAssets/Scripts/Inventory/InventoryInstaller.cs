using UnityEngine;
using Zenject;

public sealed class InventoryInstaller : MonoInstaller
{
    [SerializeField] private InventoryView _inventoryView;

    public override void InstallBindings()
    {
        Container.Bind<IInventoryService>()
            .To<InventoryService>()
            .AsSingle();

        Container.Bind<InventoryViewModel>()
            .AsSingle();

        Container.Bind<InventoryView>()
            .FromInstance(_inventoryView)
            .AsSingle();
    }
}