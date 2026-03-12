using R3;

public interface IInventoryItemViewModel
{
    string Id { get; }
    ReadOnlyReactiveProperty<string> Title { get; }
    ReadOnlyReactiveProperty<bool> IsSelected { get; }

    void Select();
    void Deselect();
    IInventoryItemModel GetModel();
}
