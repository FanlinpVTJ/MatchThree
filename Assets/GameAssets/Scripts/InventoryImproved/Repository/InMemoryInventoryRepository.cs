using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InMemoryInventoryRepository : IInventoryRepository
{
    private readonly List<IInventoryItemModel> _items = new()
    {
        new InventoryItemModel("hp_potion", "Health Potion", true),
        new InventoryItemModel("mana_potion", "Mana Potion", true),
        new InventoryItemModel("ancient_key", "Ancient Key", false),
        new InventoryItemModel("dungeon_map", "Dungeon Map", false),
    };


    public async UniTask<IReadOnlyList<IInventoryItemModel>> LoadItemsAsync()
    {
        await UniTask.WaitForSeconds(0.3f);
        return _items.ToList();
    }

    public async UniTask RemoveItemAsync(string itemId)
    {
        await UniTask.Delay(150);

        var item = _items.FirstOrDefault(x => x.Id == itemId);
        if (item != null)
            _items.Remove(item);
    }
}
