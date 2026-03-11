using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//основной сервис. Позволяет получить список предметов, использовать предмет,
//бросить предмет. Содержит текущее состояние инвентаря
public class InventoryService : IInventoryService
{
    private readonly List<InventoryItemData> _items = new()
    {
        new InventoryItemData("potion_hp", "Health Potion", true),
        new InventoryItemData("bomb", "Bomb", true),
        new InventoryItemData("key_old", "Old Key", false),
        new InventoryItemData("map", "Treasure Map", false),
    };

    public async UniTask<IReadOnlyList<InventoryItemData>> LoadAsync()
    {
        try
        {
            await UniTask.WaitForSeconds(0.5f);
            return _items.ToList();
        }
        catch (System.Exception e)
        {
            Debug.LogError("InventoryService: " + e.Message);
            return null;
        }
    }

    public async UniTask UseAsync(string itemId)
    {
        try
        {
            await UniTask.WaitForSeconds(0.5f);

            var item = _items.FirstOrDefault(x => x.Id == itemId);

            if(item != null)
            {
                if (item.IsConsumable)
                {
                    _items.Remove(item);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("InventoryService: " + e.Message);
        }
    }

    public async UniTask DropAsync(string itemId)
    {
        try
        {
            await UniTask.WaitForSeconds(0.3f);

            var item = _items.FirstOrDefault(x => x.Id == itemId);

            if (item != null)
            {
                _items.Remove(item);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("InventoryService: " + e.Message);
        }
    }
}
