using System.Collections.Generic;

public interface IInventoryModel
{
    IReadOnlyList<IInventoryItemModel> Items { get; }
    void SetItems(IReadOnlyList<IInventoryItemModel> items);
    void RemoveItem(string itemId);
}
