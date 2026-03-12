using R3;
using UnityEngine;

public class InventoryItemViewModel : IInventoryItemViewModel
{
    private readonly IInventoryItemModel _inventoryItemModel;
    
    public string Id => _inventoryItemModel.Id;

    public ReadOnlyReactiveProperty<string> Title { get; }

    public ReadOnlyReactiveProperty<bool> IsSelected { get; }

    private ReactiveProperty<string> _title;
    private ReactiveProperty<bool> _isSelected;

    public InventoryItemViewModel(IInventoryItemModel inventoryItemModel)
    {
        _inventoryItemModel = inventoryItemModel;

        _title = new ReactiveProperty<string>(_inventoryItemModel.Title);
        _isSelected = new ReactiveProperty<bool>(false);

        Title = _title
            .ToReadOnlyReactiveProperty();

        IsSelected = _isSelected
            .ToReadOnlyReactiveProperty();
    }

    public IInventoryItemModel GetModel()
    {
        return _inventoryItemModel;
    }

    public void Select()
    {
        _isSelected.Value = true;
    }

    public void Deselect()
    {
        _isSelected.Value = false;
    }
}
