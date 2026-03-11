using UnityEngine;

//основная информация предмета
public sealed class InventoryItemData
{
    public string Id { get; }
    public string Title { get; }
    public bool IsConsumable { get; }

    public InventoryItemData(string id, string title, bool isConsumable)
    {
        Id = id;
        Title = title;
        IsConsumable = isConsumable;
    }
}
