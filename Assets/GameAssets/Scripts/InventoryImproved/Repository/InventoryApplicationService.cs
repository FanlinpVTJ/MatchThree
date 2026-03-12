using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryApplicationService : IInventoryApplicationService
{
    private readonly IInventoryRepository _inventoryRepository;

    public InventoryApplicationService(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async UniTask<IReadOnlyList<IInventoryItemModel>> LoadInventoryAsync()
    {
        return await _inventoryRepository.LoadItemsAsync();
    }

    public async UniTask UseItemAsync(IInventoryItemModel itemModel)
    {
        if (itemModel == null) return;
        if (!itemModel.IsConsumable)
            return;

        await _inventoryRepository.RemoveItemAsync(itemModel.Id);
    }

    public async UniTask DropItemAsync(IInventoryItemModel itemModel)
    {
        if (itemModel == null)
            throw new ArgumentNullException(nameof(itemModel));

        await _inventoryRepository.RemoveItemAsync(itemModel.Id);
    }
}
