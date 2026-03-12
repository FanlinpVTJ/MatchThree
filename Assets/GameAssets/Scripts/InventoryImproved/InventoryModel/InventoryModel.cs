using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryModel : IInventoryModel
{
    public IReadOnlyList<IInventoryItemModel> Items => _items;

    private List<IInventoryItemModel> _items = new();

    public void RemoveItem(string itemId)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);
        if(item != null)
        {
            _items.Remove(item);
        }
    }

    public void SetItems(IReadOnlyList<IInventoryItemModel> items)
    {
        _items.Clear();
        _items.AddRange(items);
    }
}
