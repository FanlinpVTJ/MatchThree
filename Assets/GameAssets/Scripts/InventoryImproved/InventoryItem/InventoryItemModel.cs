using System;

[Serializable]
public class InventoryItemModel : IInventoryItemModel
{
    public string Id { get; }

    public string Title { get; }

    public bool IsConsumable { get; }

    public InventoryItemModel(string id, string title, bool isConsumable)
    {
        Id = id;
        Title = title;
        IsConsumable = isConsumable;
    }
}
